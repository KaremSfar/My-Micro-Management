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
using System.Text;

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
        services.AddHostedService<SqliteInitializationService>();

        services.AddDbContextFactory<AuthenticationServiceDbContext>(options =>
        {
            // TODO-KAREM: update here if deployed a real db
            var projectRoot = AppDomain.CurrentDomain.BaseDirectory;
            var dbPath = Path.Combine(projectRoot, "auth.db");

            options.UseSqlite($"DataSource={dbPath}");
        });

        services.AddIdentity<ApplicationUser, IdentityRole>()
            .AddEntityFrameworkStores<AuthenticationServiceDbContext>();

        services.AddControllers();

        var authenticationBuilder = services.AddAuthentication(options =>
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
                ValidIssuer = Configuration["Jwt:Issuer"],
                ValidAudience = Configuration["Jwt:Audience"],
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Configuration["Jwt:JwtAccessKey"]!))
            };
        })
        .AddCookie(CookieAuthenticationDefaults.AuthenticationScheme);

        if (!string.IsNullOrWhiteSpace(Configuration["googleclient_id"]))
            authenticationBuilder.AddGoogle(ConfigureGoogleSSO);

        services.AddAuthorization(b =>
        {
            // Create a new policy (along side default JWT bearer one) to exchange google cookie with JWT Token
            b.AddPolicy("google-token-exchange", pb =>
            {
                pb.AuthenticationSchemes.Clear();
                pb.AddAuthenticationSchemes(CookieAuthenticationDefaults.AuthenticationScheme)
                    .RequireAuthenticatedUser();
            });
        });

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
            options.AddPolicy("AllowLocalReact", builder =>
            {
                builder.WithOrigins("https://localhost:3000")
                    .AllowCredentials()
                    .AllowAnyMethod()
                    .AllowAnyHeader();
            });
        });
    }

    /// <summary>
    /// Register Middlewares
    /// </summary>
    /// <param name="app"></param>
    /// <param name="env"></param>
    public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
    {
        // Configure the HTTP request pipeline.
        if (env.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI();
            app.UseDeveloperExceptionPage();
            app.UseCors("AllowLocalReact");
        }
        else
        {
            app.UseCors();
        }

        app.UseRouting();

        app.UseAuthentication();
        app.UseAuthorization();

        app.UseEndpoints(endpoints =>
        {
            endpoints.MapControllers();
        });
    }

    private void ConfigureGoogleSSO(GoogleOptions options)
    {
        options.SignInScheme = CookieAuthenticationDefaults.AuthenticationScheme;
        options.ClientId = Configuration["googleclient_id"]!;
        options.ClientSecret = Configuration["googleclient_secret"]!;
        options.SaveTokens = false;

        options.Events.OnTicketReceived = async ctx =>
        {
            var userManager = ctx.HttpContext.RequestServices.GetService(typeof(UserManager<ApplicationUser>)) as UserManager<ApplicationUser>;

            var userMail = ctx.Principal?.Claims?.FirstOrDefault(c => c.Type == ClaimTypes.Email)?.Value;
            var firstName = ctx.Principal?.Claims?.FirstOrDefault(c => c.Type == ClaimTypes.GivenName)?.Value;
            var lastName = ctx.Principal?.Claims?.FirstOrDefault(c => c.Type == ClaimTypes.Surname)?.Value;

            var user = await userManager!.FindByEmailAsync(userMail);

            if (user == null)
            {
                // Create the user if user is not created yet
                var result = await userManager.CreateAsync(new ApplicationUser()
                {
                    UserName = userMail,
                    Email = userMail,
                    FirstName = firstName,
                    LastName = lastName,
                });

                if (result.Succeeded)
                {
                    ctx.Fail("Could not create user");
                }
            }
        };
    }
}
