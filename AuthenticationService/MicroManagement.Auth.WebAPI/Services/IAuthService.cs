using LanguageExt.Common;
using MicroManagement.Auth.WebAPI.DTOs;
using MicroManagement.Auth.WebAPI.Models;

namespace MicroManagement.Auth.WebAPI.Services
{
    public interface IAuthService
    {
        Task<Result<JwtAuthResult>> AuthenticateAsync(string email, string password);
        Task<JwtAuthResult> AuthenticateAsync(string email);
        Task<JwtAuthResult> RefreshTokenAsync(string refreshToken);
        Task<Result<JwtAuthResult>> RegisterAsync(RegisterDTO model);
    }
}