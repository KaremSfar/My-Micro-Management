using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MicroManagement.Application.Common
{
    public interface IAuthenticationContextProvider
    {
        Task<bool> IsAuthenticated();

        Task Login(string accessToken, string refreshToken);
        void SignOut();
    }
}
