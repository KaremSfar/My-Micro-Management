using MicroManagement.Auth.WebAPI.DTOs;
using MicroManagement.Auth.WebAPI.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace MicroManagement.Auth.WebAPI.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;

        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterDTO model)
        {
            var jwtResponse = await _authService.RegisterAsync(model);

            return jwtResponse.Match<IActionResult>(
                jwt =>
                {
                    AppendRefreshToken(jwt.RefreshToken);
                    return Ok(new JwtAccessTokenDTO(jwt.AccessToken));
                },
                failed => BadRequest(failed.Message));
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginDTO model)
        {
            var authResult = await this._authService.AuthenticateAsync(model.Email, model.Password);

            return authResult.Match<IActionResult>(
                jwt =>
                {
                    AppendRefreshToken(jwt.RefreshToken);
                    return Ok(new JwtAccessTokenDTO(jwt.AccessToken));
                },
                failed => BadRequest(failed.Message));
        }

        [HttpPost("refresh-token")]
        public async Task<IActionResult> RefreshToken([FromBody] RefreshTokenInputDto refreshToken)
        {
            var refreshResult = await this._authService.RefreshTokenAsync(refreshToken.RefreshToken);

            return refreshResult.Match<IActionResult>(
                jwt =>
                {
                    AppendRefreshToken(jwt.RefreshToken);
                    return Ok(new JwtAccessTokenDTO(jwt.AccessToken));
                },
                failed => BadRequest(failed.Message));
        }

        private void AppendRefreshToken(string refreshToken)
        {
            var cookieOptions = new CookieOptions
            {
                HttpOnly = true,
                Secure = true, // Ensure the cookie is sent over HTTPS
                SameSite = SameSiteMode.Strict, // Prevents the cookie from being sent in cross-site requests
                Expires = DateTime.UtcNow.AddDays(7) // Set the cookie to expire in 7 days
            };

            Response.Cookies.Append("refreshToken", refreshToken, cookieOptions);
        }
    }
}
