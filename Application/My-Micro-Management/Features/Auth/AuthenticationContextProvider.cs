using MicroManagement.Application.Services.Abstractions;
using Microsoft.Maui.Controls.PlatformConfiguration;
using My_Micro_Management;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace MicroManagement.Application.Common
{
    internal class AuthenticationContextProvider : IAuthenticationContextProvider
    {
        private const string RefreshTokenKey = "refresh_token";
        private const string AccessTokenKey = "jwt_token";

        private readonly IAuthenticationService _authenticationService = MauiProgram.GetService<IAuthenticationService>();
        private Timer _refreshTimer;

        public Task<string> GetAccessToken() => SecureStorage.GetAsync(AccessTokenKey);

        public async Task Login(string accessToken, string refreshToken)
        {
            await SetTokens(accessToken, refreshToken);
            StartTokensAutoRefresh();
        }

        public void SignOut()
        {
            _refreshTimer.Dispose();
            _refreshTimer = null;

            SecureStorage.Remove(AccessTokenKey);
            SecureStorage.Remove(RefreshTokenKey);
        }

        public async Task<bool> IsAuthenticated()
        {
            var jwtToken = await SecureStorage.GetAsync(AccessTokenKey);
            return jwtToken != null;
        }

        public void StartTokensAutoRefresh()
        {
            _refreshTimer = new Timer(async (state) => { await RefreshTokens(); }, null, (int)TimeSpan.FromMinutes(4).TotalMilliseconds, (int)TimeSpan.FromMinutes(4).TotalMilliseconds);
        }

        public async Task RefreshTokens()
        {
            var (accessToken, refreshToken) = await _authenticationService.RefreshTokens(await SecureStorage.GetAsync(RefreshTokenKey));
            await SetTokens(accessToken, refreshToken);
        }

        private async Task SetTokens(string accessToken, string refreshToken)
        {
            await Task.WhenAll(new[] { SecureStorage.SetAsync(AccessTokenKey, accessToken), SecureStorage.SetAsync(RefreshTokenKey, refreshToken) });
        }
    }
}
