using MicroManagement.Auth.WebAPI.Controllers;
using MicroManagement.Auth.WebAPI.DTOs;
using MicroManagement.Auth.WebAPI.Models;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace MicroManagement.Auth.WebAPI.Services
{
    public class AuthService : IAuthService
    {
        private readonly ILogger<AuthService> _logger;
        private readonly IConfiguration _configuration;

        public AuthService(ILogger<AuthService> logger, IConfiguration configuration)
        {
            _logger = logger;
            _configuration = configuration;
        }

        public async Task<JwtAuthResult> AuthenticateAsync(string email, string password)
        {
            throw new NotImplementedException();
        }
        public Task<JwtAuthResult> RefreshTokenAsync(string refreshToken)
        {
            throw new NotImplementedException();
        }

        public async Task<JwtAuthResult> RegisterAsync(RegisterDTO model)
        {
            // User Creation and saving in DB bla bla

            var claims = new List<Claim>()
            {
                new Claim(ClaimTypes.Name, model.FirstName)
            };

            string accessToken = GenerateAccessToken(claims);

            return new JwtAuthResult { AccessToken = accessToken };
        }

        // TODO-Karem: take this to a JWTManager / TokenGenerator
        private string GenerateAccessToken(IEnumerable<Claim> claims)
        {
            var secretKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]));
            var signingCredentials = new SigningCredentials(secretKey, SecurityAlgorithms.HmacSha256);

            var tokenOptions = new JwtSecurityToken(
                issuer: _configuration["Jwt:Issuer"],
                audience: _configuration["Jwt:Audience"],
                claims: claims,
                expires: DateTime.UtcNow.AddMinutes(Convert.ToDouble(_configuration["Jwt:AccessTokenDurationInMinutes"])),
                signingCredentials: signingCredentials);

            return new JwtSecurityTokenHandler().WriteToken(tokenOptions);
        }
    }
}
