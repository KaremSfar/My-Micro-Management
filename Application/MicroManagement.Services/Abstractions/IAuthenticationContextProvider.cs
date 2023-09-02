using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MicroManagement.Application.Services.Abstractions
{
    public interface IAuthenticationContextProvider
    {
        Task<string> GetAccessToken();
        Task<bool> IsAuthenticated();

        Task Login(string accessToken, string refreshToken);
        Task RefreshTokens();
        void SignOut();
    }
}
