using MicroManagement.Application.Services.Abstractions;
using Microsoft.Maui.Controls.PlatformConfiguration;
using My_Micro_Management;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MicroManagement.Application.Common
{
    internal class AuthenticationContextProvider : IAuthenticationContextProvider
    {
        private const string RefreshTokenKey = "refresh_token";
        private const string AccessTokenKey = "jwt_token";
        private readonly IAuthenticationService _authenticationService = MauiProgram.GetService<IAuthenticationService>();

        public Task<string> GetAccessToken() => SecureStorage.GetAsync(AccessTokenKey);

        public async Task<bool> IsAuthenticated()
        {
            var jwtToken = await SecureStorage.GetAsync(RefreshTokenKey);
            return jwtToken != null;
        }

        public async Task Login(string accessToken, string refreshToken)
        {
            await SetTokens(accessToken, refreshToken);

            new Timer(async (state) => { await RefreshTokens(); }, null, 0, (int)TimeSpan.FromMinutes(1).TotalMilliseconds);
        }
        public async Task RefreshTokens()
        {
            var (accessToken, refreshToken) = await _authenticationService.RefreshTokens(await SecureStorage.GetAsync(RefreshTokenKey)).ConfigureAwait(false);
            await SetTokens(accessToken, refreshToken);
        }

        public void SignOut()
        {
            SecureStorage.Remove(AccessTokenKey);
            SecureStorage.Remove(RefreshTokenKey);
        }

        private async Task SetTokens(string accessToken, string refreshToken)
        {
            await Task.WhenAll(new[] { SecureStorage.SetAsync(AccessTokenKey, accessToken), SecureStorage.SetAsync(RefreshTokenKey, refreshToken) });
        }
    }
}
