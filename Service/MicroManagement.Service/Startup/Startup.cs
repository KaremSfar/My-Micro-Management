using MicroManagement.Auth.WebAPI;
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
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Reflection;
using System.Text;

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
                c.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory, $"{Assembly.GetAssembly(typeof(ProjectDTO))!.GetName().Name}.xml"));
            });

            services.AddTransient<IProjectsRepository, SqlProjectsRepository>();
            services.AddTransient<IProjectsService, ProjectsService>();

            services.AddTransient<ITimeSessionsRepository, SqlTimeSessionsRepository>();
            services.AddTransient<ITimeSessionsService, TimeSessionsService>();
            services.AddSingleton<IUserConnectionsProvider, UserConnectionsProvider>();

            services.AddHostedService<SqliteInitializationService>();

            services.AddDbContextFactory<MyMicroManagementDbContext>(options =>
            {
                // TODO-KAREM: update here if deployed a real db
                var projectRoot = AppDomain.CurrentDomain.BaseDirectory;
                var dbPath = Path.Combine(projectRoot, "..", "..", "..", "..", "..", "SQLite", "service.db");

                options.UseSqlite($"DataSource={dbPath}", options =>
                {
                    options.MigrationsAssembly("MicroManagement.Persistence.Migrations.SQLite");
                });
            });

            services.AddCors(options =>
            {
                options.AddPolicy("allowAll", builder =>
                {
                    builder.AllowCredentials().AllowAnyHeader().AllowAnyMethod();
                });
            });
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            // Configure the HTTP request pipeline.
            if (env.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
                app.UseDeveloperExceptionPage();
                app.UseCors("allowAll");
            }
            else
            {
                app.UseCors();
            }

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseHttpsRedirection();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
                endpoints.MapHub<TimeSessionsHub>("/hub/timesessionshub");
            });
        }

    }
}
