using MicroManagement.Persistence.Abstraction.Repositories;
using MicroManagement.Persistence.EF.Configuration;
using MicroManagement.Persistence.EF.Repositories;
using MicroManagement.Services;
using MicroManagement.Services.Abstraction;
using MicroManagement.Services.Abstraction.DTOs;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using MicroManagement.Shared;
using Microsoft.IdentityModel.Tokens;
using System.Reflection;
using System.Text;
using OpenTelemetry.Trace;
using MassTransit;
using MicroManagement.Service.WebAPI.Events;
using MassTransit.Logging;
using MicroManagement.Service.Abstractions;
using MicroManagement.Service.WebAPI.Services;

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
            services.AddOpenTelemetry("mmgmt-service", Configuration["OTEL:ENDPOINT"])
                .ConfigureOpenTelemetryTracerProvider(tracerProviderBuilder =>
                {
                    tracerProviderBuilder.AddSource(DiagnosticHeaders.DefaultListenerName);
                });

            // Add services to the container.
            services.AddControllers();

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
            });

            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            services.AddEndpointsApiExplorer();
            services.AddSwaggerGen(c =>
            {
                // Include Endpoints and Controllers descriptions through XML Comments
                c.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory, $"{Assembly.GetExecutingAssembly().GetName().Name}.xml"));

                // Include DataContracts / DTOs descriptions through XML Comments
                c.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory, $"{Assembly.GetAssembly(typeof(ProjectSessionDTO))!.GetName().Name}.xml"));

                c.AddSecurityDefinition("Bearer", new Microsoft.OpenApi.Models.OpenApiSecurityScheme
                {
                    Name = "Authorization",
                    Type = Microsoft.OpenApi.Models.SecuritySchemeType.Http,
                    Scheme = "bearer",
                    BearerFormat = "JWT",
                    In = Microsoft.OpenApi.Models.ParameterLocation.Header,
                    Description = "Enter your JWT token"
                });

                c.AddSecurityRequirement(new Microsoft.OpenApi.Models.OpenApiSecurityRequirement
                {
                    {
                        new Microsoft.OpenApi.Models.OpenApiSecurityScheme
                        {
                            Reference = new Microsoft.OpenApi.Models.OpenApiReference
                            {
                                Type = Microsoft.OpenApi.Models.ReferenceType.SecurityScheme,
                                Id = "Bearer"
                            }
                        },
                        Array.Empty<string>()
                    }
                });
            });

            services.AddTransient<IProjectsRepository, SqlProjectsRepository>();
            services.AddTransient<IProjectsService, ProjectsService>();

            services.AddTransient<ITimeSessionsRepository, SqlTimeSessionsRepository>();
            services.AddTransient<ITimeSessionsService, TimeSessionsService>();
            services.AddTransient<ITimeSessionEventsPublisher, TimeSessionEventsPublisher>();

            services.AddTransient<IContextsRepository, SqlContextsRepository>();
            services.AddTransient<IContextsService, ContextService>();

            services.AddOptions<DatabaseSettings>()
                .Bind(Configuration.GetSection(DatabaseSettings.SectionName));

            var dbSettings = Configuration.GetSection(DatabaseSettings.SectionName).Get<DatabaseSettings>()!;

            services.AddDatabaseContext<MyMicroManagementDbContext>(dbSettings, SetupMigrationAssembly);

            services.AddCors(options =>
            {
                options.AddPolicy("AllowLocalReact", p => p.SetIsOriginAllowed(p => true).AllowAnyMethod().AllowAnyHeader().AllowCredentials());
            });

            // MassTransit + RabbitMQ configuration
            services.AddMassTransit(x =>
            {
                x.UsingRabbitMq((context, cfg) =>
                {
                    cfg.Host(Configuration["RabbitMq:Host"], "/", h =>
                    {
                        h.Username(Configuration["RabbitMq:Username"]);
                        h.Password(Configuration["RabbitMq:Password"]);
                    });
                    cfg.ConfigureEndpoints(context);
                });

                x.AddConsumer<UserInactivityConsumer>();
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
            });
        }
    }
}
