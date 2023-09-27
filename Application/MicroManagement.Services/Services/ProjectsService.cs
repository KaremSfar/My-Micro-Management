using MicroManagement.Application.Services.Abstraction;
using MicroManagement.Application.Services.Abstractions;
using MicroManagement.Core;
using MicroManagement.Persistence.Abstraction.Repositories;
using MicroManagement.Services.Abstraction;
using MicroManagement.Services.Abstraction.DTOs;
using RestSharp;
using RestSharp.Authenticators;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace MicroManagement.Application.Services
{
    public class ProjectsService : IProjectsService
    {
        private readonly IAuthenticationContextProvider _authenticationContextProvider;

        public ProjectsService(IAuthenticationContextProvider authenticationContextProvider)
        {
            _authenticationContextProvider = authenticationContextProvider;
        }

        public async Task<ProjectDTO> AddProject(ProjectDTO addProjectDto)
        {
            using var client = new RestClient(new RestClientOptions("https://localhost:7114")
            {
                Authenticator = new JwtAuthenticator(await this._authenticationContextProvider.GetAccessToken())
            });

            var project = await client.PostAsync<ProjectDTO>(
                new RestRequest("api/projects", Method.Post)
                    .AddBody(addProjectDto));

            return project;
        }

        public async Task<IEnumerable<ProjectDTO>> GetAll()
        {
            using var client = new RestClient(new RestClientOptions("https://localhost:7114")
            {
                Authenticator = new JwtAuthenticator(await this._authenticationContextProvider.GetAccessToken())
            });

            var projects = await client.GetAsync<List<ProjectDTO>>(new RestRequest("api/projects"));

            return projects ?? Enumerable.Empty<ProjectDTO>();
        }
    }
}
