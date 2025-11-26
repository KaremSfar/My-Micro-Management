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

        /// <summary>
        /// Endpoint to register a new user
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
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

        /// <summary>
        /// Endpoint to login existing users with basic user mail - password combination
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
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

        /// <summary>
        /// Endpoint to logout the user
        /// </summary>
        /// <returns></returns>
        [HttpPost("logout")]
        public IActionResult Logout()
        {
            AppendRefreshToken(string.Empty);
            return Ok();
        }

        /// <summary>
        /// Refresh token endpoint, based on the given refreshToken returns a new pair of access and refresh tokens
        /// </summary>
        /// <param name="refreshToken"></param>
        /// <returns></returns>
        [HttpPost("refresh-token")]
        public async Task<ActionResult<JwtAccessTokenDTO>> RefreshToken()
        {
            var refreshToken = Request.Cookies["refreshToken"];

            if (string.IsNullOrEmpty(refreshToken))
            {
                return BadRequest("Refresh token is missing.");
            }

            var refreshResult = await this._authService.RefreshTokenAsync(refreshToken);

            return refreshResult.Match<ActionResult<JwtAccessTokenDTO>>(
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
                Secure = true, // Ensure the cookie is sent over HTTPS
                SameSite = SameSiteMode.Strict,
                Expires = DateTimeOffset.UtcNow.AddDays(7),
                HttpOnly = true
            };

            Response.Cookies.Append("refreshToken", refreshToken, cookieOptions);
        }
    }
}
