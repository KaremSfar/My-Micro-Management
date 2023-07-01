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
            return Challenge(GoogleDefaults.AuthenticationScheme);
        }

        [HttpGet("google-response")]
        public async Task<IActionResult> GoogleResponse()
        {
            // This endpoint is still not hit for some reasons,
            return Ok();
        }
    }
}
