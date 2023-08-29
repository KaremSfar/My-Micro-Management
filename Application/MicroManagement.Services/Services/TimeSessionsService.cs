using MicroManagement.Application.Services.Abstraction;
using MicroManagement.Persistence.Abstraction.Repositories;
using MicroManagement.Services.Abstraction.DTOs;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MicroManagement.Application.Services
{
    public class TimeSessionsService : ITimeSessionsService
    {
        public TimeSessionsService()
        {
        }

        public async Task<TimeSessionDTO> AddTimeSession(TimeSessionDTO timeSessionDTO)
        {
            var client = new RestClient(new RestClientOptions("https://localhost:7114"));
            var timeSession = await client.PostAsync<TimeSessionDTO>(
                new RestRequest("api/timeSessions", Method.Post)
                    .AddBody(timeSessionDTO));

            return timeSession;
        }

        public async Task<IEnumerable<TimeSessionDTO>> GetAll()
        {
            var client = new RestClient(new RestClientOptions("https://localhost:7114"));
            var projects = await client.GetAsync<List<TimeSessionDTO>>(new RestRequest("api/timeSessions"));

            return projects ?? Enumerable.Empty<TimeSessionDTO>();
        }
    }
}
