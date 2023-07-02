using MicroManagement.Auth.WebAPI.Models;
using MicroManagement.Auth.WebAPI.Persistence;
using MicroManagement.Auth.WebAPI.Services;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using System.Security.Claims;
using System.Text;

namespace MicroManagement.Auth.WebAPI
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddDbContext<AuthenticationServiceDbContext>(options =>
            {
                options.UseSqlite(@"Data Source=C:\Repos\Temp\MyAuthDB-dev.db");
            });

            services.AddIdentity<ApplicationUser, IdentityRole>()
                .AddEntityFrameworkStores<AuthenticationServiceDbContext>();

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
                    ValidIssuer = Configuration["Jwt:Issuer"],
                    ValidAudience = Configuration["Jwt:Audience"],
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Configuration["Jwt:Key"])) // dotnet user-secrets set "Jwt:Key": to remove
                };
            })
            .AddCookie(CookieAuthenticationDefaults.AuthenticationScheme)
            .AddGoogle(options =>
            {
                options.SignInScheme = CookieAuthenticationDefaults.AuthenticationScheme;
                options.ClientId = Configuration["google:client-id"]!;
                options.ClientSecret = Configuration["google:client-secret"]!;
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
            });

            services.AddAuthorization(b =>
            {
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
            services.AddSwaggerGen();
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            // Configure the HTTP request pipeline.
            if (env.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
                app.UseDeveloperExceptionPage();
            }

            app.UseRouting();
            app.UseCors();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseHttpsRedirection();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
