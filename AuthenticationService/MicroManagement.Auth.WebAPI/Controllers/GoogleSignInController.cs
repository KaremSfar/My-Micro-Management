using Microsoft.AspNetCore.Authentication.Google;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authentication.Cookies;
using System.Security.Claims;

namespace MicroManagement.Auth.WebAPI.Controllers
{
    [ApiController]
    [Route("google")]
    public class GoogleSignInController : ControllerBase
    {
        [HttpGet("signin-google")]
        public IActionResult SignInWithGoogle()
        {
            var properties = new AuthenticationProperties { RedirectUri = "https://localhost:44325/google/google-response" };
            return Challenge(properties, GoogleDefaults.AuthenticationScheme);
        }

        [HttpGet("google-response")]
        public async Task<IActionResult> GoogleResponse()
        {
            var result = await HttpContext.AuthenticateAsync(CookieAuthenticationDefaults.AuthenticationScheme);

            if (result?.Succeeded != true)
            {
                return BadRequest("Google authentication failed");
            }

            // Get the user's information from the result
            var claims = result.Principal.Identities
                .FirstOrDefault(y => y.AuthenticationType == CookieAuthenticationDefaults.AuthenticationScheme)?
                .Claims;

            var email = claims?.FirstOrDefault(x => x.Type == ClaimTypes.Email)?.Value;

            return Ok();
        }
    }
}
