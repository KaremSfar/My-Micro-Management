using MicroManagement.Auth.WebAPI.DTOs;
using MicroManagement.Auth.WebAPI.Models;

namespace MicroManagement.Auth.WebAPI.Services
{
    public class AuthService : IAuthService
    {
        public Task<JwtAuthResult> AuthenticateAsync(string email, string password)
        {
            throw new NotImplementedException();
        }

        public Task<JwtAuthResult> RefreshTokenAsync(string refreshToken)
        {
            throw new NotImplementedException();
        }

        public Task<ApplicationUser> RegisterAsync(RegisterDTO model)
        {
            throw new NotImplementedException();
        }
    }
}
