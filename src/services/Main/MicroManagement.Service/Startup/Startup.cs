using MicroManagement.Persistence.Abstraction.Repositories;
using MicroManagement.Persistence.EF.Configuration;
using MicroManagement.Persistence.EF.Repositories;
using MicroManagement.Service.WebAPI.Hubs;
using MicroManagement.Service.WebAPI.Services;
using MicroManagement.Services;
using MicroManagement.Services.Abstraction;
using MicroManagement.Services.Abstraction.DTOs;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.WebSockets;
using MicroManagement.Shared;
using Microsoft.IdentityModel.Tokens;
using System.Reflection;
using System.Text;
using Microsoft.Extensions.Configuration;
using OpenTelemetry.Logs;
using OpenTelemetry.Resources;
using OpenTelemetry.Trace;

namespace MicroManagement.Service
{
    public class Startup
    {
        public IConfiguration Configuration { get; }

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public void ConfigureServices(IServiceCollection services)
        {
            AddOpenTelemetry(services);

            // Add services to the container.
            services.AddControllers();

            services.AddSignalR();

            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    ValidIssuer = Configuration["Jwt:Issuer"]!,
                    ValidAudience = Configuration["Jwt:Audience"]!,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Configuration["Jwt:JwtAccessKey"]!))
                };

                options.Events = new JwtBearerEvents
                {
                    OnMessageReceived = context =>
                    {
                        var accessToken = context.Request.Query["access_token"];
                        var path = context.HttpContext.Request.Path;

                        // Set Access token for SignalR hubs
                        if (!string.IsNullOrEmpty(accessToken) && path.StartsWithSegments("/hub/timesessionshub"))
                            context.Token = accessToken;

                        return Task.CompletedTask;
                    }
                };
            });

            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            services.AddEndpointsApiExplorer();
            services.AddSwaggerGen(c =>
            {
                // Include Endpoints and Controllers descriptions through XML Comments
                c.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory, $"{Assembly.GetExecutingAssembly().GetName().Name}.xml"));

                // Include DataContracts / DTOs descriptions through XML Comments
                c.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory, $"{Assembly.GetAssembly(typeof(ProjectSessionDTO))!.GetName().Name}.xml"));
            });

            services.AddTransient<IProjectsRepository, SqlProjectsRepository>();
            services.AddTransient<IProjectsService, ProjectsService>();

            services.AddTransient<ITimeSessionsRepository, SqlTimeSessionsRepository>();
            services.AddTransient<ITimeSessionsService, TimeSessionsService>();
            services.AddSingleton<IUserConnectionsProvider, UserConnectionsProvider>();

            services.AddOptions<DatabaseSettings>()
                .Bind(Configuration.GetSection(DatabaseSettings.SectionName));

            var dbSettings = Configuration.GetSection(DatabaseSettings.SectionName).Get<DatabaseSettings>()!;

            services.AddDatabaseContext<MyMicroManagementDbContext>(dbSettings, SetupMigrationAssembly);

            services.AddCors(options =>
            {
                options.AddPolicy("AllowLocalReact", p => p.SetIsOriginAllowed(p => true).AllowAnyMethod().AllowAnyHeader().AllowCredentials());
            });

            void SetupMigrationAssembly(DbSetupOptions options)
            {

                var assembly = dbSettings.DatabaseType switch
                {
                    "postgres" => typeof(Persistence.Migrations.Postgres.Migrations.InitialCreate).Assembly.GetName().Name!,
                    "sqlite" => typeof(Persistence.SQLite.MigrationsApplier.Migrations.InitialCreate).Assembly.GetName().Name!,
                    _ => throw new ArgumentException("Choose Database type in configuration")
                };
                Console.WriteLine($"Using migrations assembly: {assembly}");

                options.MigrationsAssembly(assembly);
            }
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            app.UseCors("AllowLocalReact");

            // Configure the HTTP request pipeline.
            if (env.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
                app.UseDeveloperExceptionPage();
            }

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
                endpoints.MapHub<TimeSessionsHub>("/hub/timesessionshub");
            });
        }

        private void AddOpenTelemetry(IServiceCollection services)
        {
            if (Configuration["OTEL:JAEGER_URL"] == null)
                return;

            services.AddOpenTelemetry()
                .WithTracing(tracerProviderBuilder =>
                {
                    tracerProviderBuilder
                        .SetResourceBuilder(ResourceBuilder.CreateDefault()
                            .AddService(serviceName: "mmgmt-service"))
                        .AddAspNetCoreInstrumentation(options =>
                        {
                            options.RecordException = true;
                        })
                        .AddHttpClientInstrumentation(options =>
                        {
                            options.RecordException = true;
                        })
                        .AddOtlpExporter(options =>
                        {
                            options.Endpoint = new Uri(Configuration["OTEL:JAEGER_URL"]);
                        })
                        .AddConsoleExporter();
                }).WithLogging(loggerOptions =>
                {
                    loggerOptions.AddOtlpExporter(options =>
                    {
                        options.Endpoint = new Uri(Configuration["OTEL:JAEGER_URL"]);
                    });
                });
        }
    }
}
