using Coravel;
using MassTransit;
using MicroManagement.Activity.WebAPI.Events;
using MicroManagement.Activity.WebAPI.Hubs;
using MicroManagement.Activity.WebAPI.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.IdentityModel.Tokens;
using StackExchange.Redis;
using System.Text;

namespace MicroManagement.Activity.WebAPI;

public class Startup
{
    public IConfiguration Configuration { get; }

    public Startup(IConfiguration configuration)
    {
        Configuration = configuration;
    }

    public void ConfigureServices(IServiceCollection services)
    {
        services.AddControllers();
        services.AddEndpointsApiExplorer();
        services.AddSwaggerGen();

        services.AddSignalR();

        services.AddSingleton<IConnectionMultiplexer>(sp =>
            ConnectionMultiplexer.Connect(
                Configuration.GetSection("Redis")["Configuration"]));

        services.AddSingleton<IUserActivityManager, UserActivityManager>();
        services.AddSingleton<IUserConnectionRepository, UserConnectionRepository>();
        services.AddTransient<UserInactivityMonitor>();

        services.AddAuthentication()
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

            x.AddConsumer<TimeSessionEventsConsumer>();
        });

        services.AddScheduler();

        services.AddCors(options =>
        {
            options.AddPolicy("AllowLocalReact", p => p.SetIsOriginAllowed(p => true).AllowAnyMethod().AllowAnyHeader().AllowCredentials());
        });
    }

    public void Configure(IApplicationBuilder app, IWebHostEnvironment env, IServiceProvider serviceProvider)
    {
        app.UseCors("AllowLocalReact");

        // Configure the HTTP request pipeline.
        if (env.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI();
        }

        app.UseRouting();

        app.UseAuthentication();
        app.UseAuthorization();

        app.UseEndpoints(endpoints =>
        {
            endpoints.MapControllers();
            endpoints.MapHub<UserActivityHub>("/hub/timesessionshub");
        });

        serviceProvider.UseScheduler(sc =>
        {
            sc.Schedule<UserInactivityMonitor>()
                .EveryFiveMinutes();
        });
    }
}
