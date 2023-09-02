using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MicroManagement.Application.Services.Abstractions
{
    public interface IAuthenticationContextProvider
    {
        // TODO-KAREM: strip out this interface, it should be SetTokens, GetToken, and Reset Tokens
        Task<string> GetAccessToken();
        Task<bool> IsAuthenticated();

        Task Login(string accessToken, string refreshToken);
        Task RefreshTokens();
        void SignOut();
    }
}
