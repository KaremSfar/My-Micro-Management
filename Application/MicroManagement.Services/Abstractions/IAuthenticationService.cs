using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MicroManagement.Application.Services.Abstractions
{
    public interface IAuthenticationService
    {
        Task<(string jwtAccessToken, string jwtRefreshToken)> Login(string email, string password);
        Task<(string jwtAccessToken, string jwtRefreshToken)> RefreshTokens(string refreshToken);
    }
}
