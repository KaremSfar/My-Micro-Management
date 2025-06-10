using LanguageExt.Common;
using MicroManagement.Auth.WebAPI.DTOs;
using MicroManagement.Auth.WebAPI.Models;

namespace MicroManagement.Auth.WebAPI.Services
{
    public interface IAuthService
    {
        Task<Result<JwtTokens>> AuthenticateAsync(string email, string password);
        Task<JwtTokens> AuthenticateAsync(string email);
        Task<Result<JwtTokens>> RefreshTokenAsync(string refreshToken);
        Task<Result<JwtTokens>> RegisterAsync(RegisterDTO model);
    }
}