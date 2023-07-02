using MicroManagement.Auth.WebAPI.Controllers;
using MicroManagement.Auth.WebAPI.DTOs;
using MicroManagement.Auth.WebAPI.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.ApplicationModels;
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
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly UserManager<ApplicationUser> _userManager;

        public AuthService(ILogger<AuthService> logger, IConfiguration configuration, SignInManager<ApplicationUser> signInManager, UserManager<ApplicationUser> userManager)
        {
            _logger = logger;
            _configuration = configuration;
            _signInManager = signInManager;
            _userManager = userManager;
        }

        public async Task<JwtAuthResult> AuthenticateAsync(string email, string password)
        {
            var user = await _userManager.FindByEmailAsync(email);
            if (user == null)
            {
                throw new InvalidOperationException("Verify Creds");
            }

            var result = await _signInManager.CheckPasswordSignInAsync(user, password, false);

            if (!result.Succeeded)
            {
                throw new InvalidOperationException("Verify Creds");
            }

            string accessToken = GenerateAccessToken(user);
            string refreshToken = GenerateRefreshToken(user);

            return new JwtAuthResult { AccessToken = accessToken, RefreshToken = refreshToken };
        }

        public async Task<JwtAuthResult> AuthenticateAsync(string userMail)
        {
            var user = await _userManager.FindByEmailAsync(userMail);
            if (user == null)
            {
                throw new InvalidOperationException("Verify Creds");
            }

            string accessToken = GenerateAccessToken(user);
            string refreshToken = GenerateRefreshToken(user);

            return new JwtAuthResult { AccessToken = accessToken, RefreshToken = refreshToken };
        }

        public async Task<JwtAuthResult> RegisterAsync(RegisterDTO model)
        {
            var user = new ApplicationUser()
            {
                Email = model.Email,
                FirstName = model.FirstName,
                LastName = model.LastName,
                UserName = model.Email,
            };

            var result = await _userManager.CreateAsync(user, password: model.Password);

            if (!result.Succeeded)
            {
                throw new InvalidOperationException(result.Errors.First().ToString());
            }

            string accessToken = GenerateAccessToken(user);
            string refreshToken = GenerateRefreshToken(user);

            return new JwtAuthResult { AccessToken = accessToken, RefreshToken = refreshToken };
        }

        public async Task<JwtAuthResult> RefreshTokenAsync(string refreshTokenInput)
        {
            ClaimsPrincipal tokenClaims = ValidateRefreshToken(refreshTokenInput);

            string userMail = tokenClaims.Claims
                .Where(c => c.Type == ClaimTypes.Email)
                .Single()
                .Value;

            var user = await _userManager.FindByEmailAsync(userMail);

            string newAccessToken = GenerateAccessToken(user!);
            string newRefreshToken = GenerateRefreshToken(user!);

            return new JwtAuthResult { AccessToken = newAccessToken, RefreshToken = newRefreshToken };
        }

        #region Token Manipulation
        private ClaimsPrincipal ValidateRefreshToken(string refreshTokenInput)
        {
            var secretKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]!));

            var validationParam = new TokenValidationParameters()
            {
                IssuerSigningKey = secretKey,
                ValidAudience = _configuration["Jwt:Audience"],
                ValidIssuer = _configuration["Jwt:Issuer"],
            };

            var handler = new JwtSecurityTokenHandler();

            var tokenClaims = handler.ValidateToken(refreshTokenInput, validationParam, out _);
            return tokenClaims;
        }
        private string GenerateAccessToken(ApplicationUser user)
        {
            var claims = new List<Claim>()
            {
                new Claim(ClaimTypes.Email, user.Email!)
            };

            var secretKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]!));
            var signingCredentials = new SigningCredentials(secretKey, SecurityAlgorithms.HmacSha256);

            var tokenOptions = new JwtSecurityToken(
                issuer: _configuration["Jwt:Issuer"],
                audience: _configuration["Jwt:Audience"],
                claims: claims,
                expires: DateTime.UtcNow.AddMinutes(Convert.ToDouble(_configuration["Jwt:AccessTokenDurationInMinutes"])),
                signingCredentials: signingCredentials);

            return new JwtSecurityTokenHandler().WriteToken(tokenOptions);
        }
        private string GenerateRefreshToken(ApplicationUser user)
        {
            var claims = new List<Claim>()
            {
                new Claim(ClaimTypes.Email, user.Email!)
            };

            var secretKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]!));
            var signingCredentials = new SigningCredentials(secretKey, SecurityAlgorithms.HmacSha256);

            var tokenOptions = new JwtSecurityToken(
                issuer: _configuration["Jwt:Issuer"],
                audience: _configuration["Jwt:Audience"],
                claims: claims,
                expires: DateTime.UtcNow.AddDays(Convert.ToDouble(_configuration["Jwt:RefreshTokenDurationInDays"])),
                signingCredentials: signingCredentials);

            return new JwtSecurityTokenHandler().WriteToken(tokenOptions);
        }
        #endregion
    }
}
