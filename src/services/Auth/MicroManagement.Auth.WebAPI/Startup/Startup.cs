using MicroManagement.Auth.WebAPI.DTOs;
using MicroManagement.Auth.WebAPI.Models;
using MicroManagement.Auth.WebAPI.Persistence;
using MicroManagement.Auth.WebAPI.Services;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.Google;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.Reflection;
using System.Security.Claims;
using MicroManagement.Shared;
using System.Text;
using Microsoft.Extensions.Configuration;
using Microsoft.AspNetCore.HttpOverrides;

namespace MicroManagement.Auth.WebAPI;

public class Startup
{
    public IConfiguration Configuration { get; }

    public Startup(IConfiguration configuration)
    {
        Configuration = configuration;
    }

    /// <summary>
    /// Register Services
    /// </summary>
    /// <param name="services"></param>
    public void ConfigureServices(IServiceCollection services)
    {
        var dbSettings = Configuration.GetSection(DatabaseSettings.SectionName).Get<DatabaseSettings>()!;

        services.AddDatabaseContext<AuthenticationServiceDbContext>(dbSettings,
                SetupMigrationAssembly);

        services.AddIdentity<ApplicationUser, IdentityRole>()
            .AddEntityFrameworkStores<AuthenticationServiceDbContext>();

        services.AddOptions<DatabaseSettings>()
            .Bind(Configuration.GetSection(DatabaseSettings.SectionName));

        services.AddControllers();

        services.Configure<ForwardedHeadersOptions>(options =>
        {
            options.ForwardedHeaders = ForwardedHeaders.XForwardedFor |
                                      ForwardedHeaders.XForwardedProto |
                                      ForwardedHeaders.XForwardedHost;
            options.KnownNetworks.Clear();
            options.KnownProxies.Clear();
        });

        var authenticationBuilder = services.AddAuthentication();

        if (!string.IsNullOrWhiteSpace(Configuration["googleclient_id"]))
            authenticationBuilder.AddGoogle(ConfigureGoogleSSO);

        services.AddScoped<IAuthService, AuthService>();

        // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
        services.AddEndpointsApiExplorer();
        services.AddSwaggerGen(c =>
        {
            // Include Endpoints and Controllers descriptions through XML Comments
            c.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory, $"{Assembly.GetExecutingAssembly().GetName().Name}.xml"));

            // Include DataContracts / DTOs descriptions through XML Comments
            c.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory, $"{Assembly.GetAssembly(typeof(JwtAccessTokenDTO))!.GetName().Name}.xml"));
        });

        services.AddCors(options =>
        {
            options.AddPolicy("AllowLocalReact",
                p => p.SetIsOriginAllowed(p => true).AllowAnyMethod().AllowAnyHeader().AllowCredentials());
        });

        void SetupMigrationAssembly(DbSetupOptions options)
        {
            var dbSettings = Configuration.GetSection(DatabaseSettings.SectionName).Get<DatabaseSettings>()!;

            var assembly = dbSettings.DatabaseType switch
            {
                "postgres" => typeof(Migrations.Postgres.Migrations.InitialCreate).Assembly.GetName().Name!,
                "sqlite" => typeof(Migrations.SQLite.Migrations.InitialCreate).Assembly.GetName().Name!,
                _ => throw new ArgumentException("Choose Database type in configuration")
            };

            options.MigrationsAssembly(assembly);
        }
    }

    /// <summary>
    /// Register Middlewares
    /// </summary>
    /// <param name="app"></param>
    /// <param name="env"></param>
    public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
    {
        app.UseForwardedHeaders();

        app.UseCors("AllowLocalReact");

        // Configure the HTTP request pipeline.
        if (env.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI();
            app.UseDeveloperExceptionPage();
        }

        app.UseRouting();

        app.UseCookiePolicy();
        app.UseAuthentication();
        app.UseAuthorization();

        app.UseEndpoints(endpoints =>
        {
            endpoints.MapControllers();
        });
    }

    private void ConfigureGoogleSSO(GoogleOptions options)
    {
        options.ClientId = Configuration["googleclient_id"]!;
        options.ClientSecret = Configuration["googleclient_secret"]!;
    }
}
