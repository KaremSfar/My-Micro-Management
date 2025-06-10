using Microsoft.AspNetCore.Authentication.Google;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authentication.Cookies;
using System.Security.Claims;
using MicroManagement.Auth.WebAPI.DTOs;
using MicroManagement.Auth.WebAPI.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using MicroManagement.Auth.WebAPI.Services;

namespace MicroManagement.Auth.WebAPI.Controllers
{
    [ApiController]
    public class GoogleSignInController : ControllerBase
    {
        private readonly IAuthService _authenticationService;

        public GoogleSignInController(IAuthService authenticationService)
        {
            _authenticationService = authenticationService;
        }

        [HttpGet("google-login")]
        public IActionResult GoogleLogin([FromQuery] string returnUrl)
        {
            var properties = new AuthenticationProperties
            {
                RedirectUri = Url.Action(nameof(GoogleCallback)),
                Items = { { "returnUrl", returnUrl } }
            };
            return Challenge(properties, GoogleDefaults.AuthenticationScheme);
        }

        [HttpGet("google-callback")]
        public async Task<IActionResult> GoogleCallback()
        {
            var result = await HttpContext.AuthenticateAsync(GoogleDefaults.AuthenticationScheme);

            if (!result.Succeeded)
                return BadRequest();

            var userManager = HttpContext.RequestServices.GetService(typeof(UserManager<ApplicationUser>)) as UserManager<ApplicationUser>;

            var userMail = result.Principal.Claims?.FirstOrDefault(c => c.Type == ClaimTypes.Email)?.Value;
            var firstName = result.Principal?.Claims?.FirstOrDefault(c => c.Type == ClaimTypes.GivenName)?.Value;
            var lastName = result.Principal?.Claims?.FirstOrDefault(c => c.Type == ClaimTypes.Surname)?.Value;

            var user = await userManager!.FindByEmailAsync(userMail);

            if (user == null)
            {
                // Create the user if user is not created yet
                var creationResult = await userManager.CreateAsync(new ApplicationUser()
                {
                    UserName = userMail,
                    Email = userMail,
                    FirstName = firstName,
                    LastName = lastName,
                });

                if (!creationResult.Succeeded)
                    return BadRequest("Could not create user");

                user = await userManager!.FindByEmailAsync(userMail);
            }

            var connection = await _authenticationService.AuthenticateAsync(user.Email);

            AppendRefreshToken(connection.RefreshToken);

            var returnUrl = result.Properties.Items["returnUrl"];

            return Redirect($"{returnUrl}/google-callback");
        }

        private void AppendRefreshToken(string refreshToken)
        {
            var cookieOptions = new CookieOptions
            {
                Secure = true, // Ensure the cookie is sent over HTTPS
                SameSite = SameSiteMode.None, // Prevents the cookie from being sent in cross-site requests
                Expires = DateTimeOffset.UtcNow.AddDays(7)
            };

            Response.Cookies.Append("refreshToken", refreshToken, cookieOptions);
        }
    }
}
