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
                jwt => Ok(jwtResponse),
                failed => BadRequest(failed.Message));
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginDTO model)
        {
            var authResult = await this._authService.AuthenticateAsync(model.Email, model.Password);

            return authResult.Match<IActionResult>(
                jwt => Ok(jwt),
                failed => BadRequest(failed));
        }

        [HttpPost("refresh-token")]
        public async Task<IActionResult> RefreshToken([FromBody] RefreshTokenInputDto refreshToken)
        {
            return Ok(await _authService.RefreshTokenAsync(refreshToken.RefreshToken));
        }
    }
}
