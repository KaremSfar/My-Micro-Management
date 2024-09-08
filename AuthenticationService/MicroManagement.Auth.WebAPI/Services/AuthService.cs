using LanguageExt.Common;
using LanguageExt.Pretty;
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

        public async Task<Result<JwtTokens>> AuthenticateAsync(string email, string password)
        {
            var user = await _userManager.FindByEmailAsync(email);
            if (user == null)
            {
                _logger.LogWarning("Attempted signup for unregistered email: {Email}", email);
                return new Result<JwtTokens>(new InvalidOperationException("Verify Creds"));
            }

            var result = await _signInManager.CheckPasswordSignInAsync(user, password, false);

            if (!result.Succeeded)
            {
                _logger.LogWarning("Failed login attempt for user: {UserId}", user.Id);
                return new Result<JwtTokens>(new InvalidOperationException("Verify Creds"));
            }

            string accessToken = GenerateAccessToken(user);
            string refreshToken = GenerateRefreshToken(user);

            return new JwtTokens { AccessToken = accessToken, RefreshToken = refreshToken };
        }

        public async Task<JwtTokens> AuthenticateAsync(string userMail)
        {
            var user = await _userManager.FindByEmailAsync(userMail);
            if (user == null)
            {
                throw new InvalidOperationException("Verify Creds");
            }

            string accessToken = GenerateAccessToken(user);
            string refreshToken = GenerateRefreshToken(user);

            return new JwtTokens { AccessToken = accessToken, RefreshToken = refreshToken };
        }

        public async Task<Result<JwtTokens>> RegisterAsync(RegisterDTO model)
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
                return new Result<JwtTokens>(new InvalidOperationException(result.Errors.First().ToString()));
            }

            string accessToken = GenerateAccessToken(user);
            string refreshToken = GenerateRefreshToken(user);

            return new JwtTokens { AccessToken = accessToken, RefreshToken = refreshToken };
        }

        public async Task<Result<JwtTokens>> RefreshTokenAsync(string refreshTokenInput)
        {
            try
            {
                ClaimsPrincipal tokenClaims = ValidateRefreshToken(refreshTokenInput);

                string userMail = tokenClaims.Claims
                    .Where(c => c.Type == ClaimTypes.Email)
                    .Single()
                    .Value;

                var user = await _userManager.FindByEmailAsync(userMail);

                if (user == null)
                {
                    _logger.LogWarning("Attempted refresh Token for inexistant user: {Email}", userMail);
                    return new Result<JwtTokens>(new InvalidOperationException("Verify Creds"));
                }

                string newAccessToken = GenerateAccessToken(user);
                string newRefreshToken = GenerateRefreshToken(user);

                return new JwtTokens { AccessToken = newAccessToken, RefreshToken = newRefreshToken };
            }
            catch (Exception ex)
            {
                _logger.LogWarning("Attempted refresh for invalid Token: {token}", refreshTokenInput);
                return new Result<JwtTokens>(ex);
            }
        }

        #region Token Manipulation
        private ClaimsPrincipal ValidateRefreshToken(string refreshTokenInput)
        {
            var secretKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:JwtRefreshKey"]!));

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
                new Claim(ClaimTypes.Email, user.Email!),
                new Claim(JwtRegisteredClaimNames.Sub, user.Id!)
            };

            var secretKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:JwtAccessKey"]!));
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

            var secretKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:JwtRefreshKey"]!));
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
