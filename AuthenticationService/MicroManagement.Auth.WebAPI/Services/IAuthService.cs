using MicroManagement.Auth.WebAPI.DTOs;
using MicroManagement.Auth.WebAPI.Models;

namespace MicroManagement.Auth.WebAPI.Services
{
    internal interface IAuthService
    {
        Task<JwtAuthResult> AuthenticateAsync(string email, string password);
        Task<JwtAuthResult> RefreshTokenAsync(string refreshToken);
        Task<ApplicationUser> RegisterAsync(RegisterDTO model);
    }
}