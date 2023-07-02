using Microsoft.AspNetCore.Authentication.Google;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authentication.Cookies;
using System.Security.Claims;

namespace MicroManagement.Auth.WebAPI.Controllers
{
    [ApiController]
    [Route("/")]
    public class GoogleSignInController : ControllerBase
    {
        [HttpGet("google/google-link")]
        public async Task<IResult> GoogleLink()
        {
            return Results.Challenge(
                properties: new AuthenticationProperties
                {
                    RedirectUri = "https://localhost:44325/WeatherForecast"
                },
                authenticationSchemes: new List<string> { GoogleDefaults.AuthenticationScheme }
                );
        }
    }
}
