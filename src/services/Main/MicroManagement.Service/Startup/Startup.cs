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
using MicroManagement.Persistence.Migrations.Postgres;
using MicroManagement.Persistence.Migrations.Postgres.Migrations;

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

            services.AddDbContext<MyMicroManagementDbContext, InitialCreate>(Configuration);

            services.AddOptions<DatabaseSettings>()
                .Bind(Configuration.GetSection(DatabaseSettings.SectionName));

            services.AddCors(options =>
            {
                options.AddPolicy("AllowLocalReact", p => p.SetIsOriginAllowed(p => true).AllowAnyMethod().AllowAnyHeader().AllowCredentials());
            });
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
    }
}
