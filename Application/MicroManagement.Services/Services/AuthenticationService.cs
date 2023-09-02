using MicroManagement.Application.Services.Abstractions;
using MicroManagement.Auth.WebAPI.DTOs;
using MicroManagement.Services.Abstraction.DTOs;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MicroManagement.Application.Services.Services
{
    public class AuthenticationService : IAuthenticationService
    {
        public async Task<(string jwtAccessToken, string jwtRefreshToken)> Login(string email, string password)
        {
            var client = new RestClient(new RestClientOptions("https://localhost:44325"));
            var response = await client.ExecuteAsync<JwtAccessTokenDTO>(
                new RestRequest("auth/login", Method.Post)
                    .AddBody(new LoginDTO { Email = email, Password = password })
                );

            var accessToken = response.Data?.AccessToken;
            var refreshToken = response.Cookies
                .Where(x => x.Name == "refreshToken")
                .Select(x => x.Value.ToString())
                .FirstOrDefault();

            return (accessToken, refreshToken);
        }

        public async Task<(string jwtAccessToken, string jwtRefreshToken)> RefreshTokens(string refreshToken)
        {
            var client = new RestClient(new RestClientOptions("https://localhost:44325"));
            var response = await client.ExecuteAsync<JwtAccessTokenDTO>(
                new RestRequest("auth/refresh-token", Method.Post)
                    .AddBody(new RefreshTokenInputDto { RefreshToken = refreshToken })
                );

            var accessToken = response.Data?.AccessToken;
            var newRefreshToken = response.Cookies
                .Where(x => x.Name == "refreshToken")
                .Select(x => x.Value.ToString())
                .FirstOrDefault();

            return (accessToken, newRefreshToken);
        }
    }
}
