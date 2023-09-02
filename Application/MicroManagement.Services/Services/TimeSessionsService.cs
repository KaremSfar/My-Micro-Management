using MicroManagement.Application.Services.Abstraction;
using MicroManagement.Application.Services.Abstractions;
using MicroManagement.Persistence.Abstraction.Repositories;
using MicroManagement.Services.Abstraction.DTOs;
using RestSharp;
using RestSharp.Authenticators;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MicroManagement.Application.Services
{
    public class TimeSessionsService : ITimeSessionsService
    {
        private readonly IAuthenticationContextProvider _authenticationContextProvider;

        public TimeSessionsService(IAuthenticationContextProvider authenticationContextProvider)
        {
            _authenticationContextProvider = authenticationContextProvider;
        }

        public async Task<TimeSessionDTO> AddTimeSession(TimeSessionDTO timeSessionDTO)
        {
            using var client = new RestClient(new RestClientOptions("https://localhost:7114")
            {
                Authenticator = new JwtAuthenticator(await this._authenticationContextProvider.GetAccessToken())
            });

            var timeSession = await client.PostAsync<TimeSessionDTO>(
                new RestRequest("api/timeSessions", Method.Post)
                    .AddBody(timeSessionDTO));

            return timeSession;
        }

        public async Task<IEnumerable<TimeSessionDTO>> GetAll()
        {
            using var client = new RestClient(new RestClientOptions("https://localhost:7114")
            {
                Authenticator = new JwtAuthenticator(await this._authenticationContextProvider.GetAccessToken())
            });

            var projects = await client.GetAsync<List<TimeSessionDTO>>(new RestRequest("api/timeSessions"));

            return projects ?? Enumerable.Empty<TimeSessionDTO>();
        }
    }
}
