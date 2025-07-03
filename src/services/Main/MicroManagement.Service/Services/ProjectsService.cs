using MicroManagement.Core;
using MicroManagement.Persistence.Abstraction.Repositories;
using MicroManagement.Services.Abstraction;
using MicroManagement.Services.Abstraction.DTOs;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace MicroManagement.Services
{
    public class ProjectsService : IProjectsService
    {
        private IProjectsRepository _projectsRepo;
        private ITimeSessionsRepository _timeSessionsRepository;

        public ProjectsService(IProjectsRepository projectsRepo, ITimeSessionsRepository timeSessionsRepository)
        {
            _projectsRepo = projectsRepo;
            _timeSessionsRepository = timeSessionsRepository;
        }

        public async Task<GetProjectDTO> AddProject(Guid userId, CreateProjectDTO addProjectDto)
        {
            var projectToAdd = new Project(Guid.NewGuid(), userId, addProjectDto.Name, addProjectDto.Color);

            await _projectsRepo.AddProjectAsync(projectToAdd);

            return new GetProjectDTO { Id = projectToAdd.Id, Name = projectToAdd.Name, Color = projectToAdd.Color };
        }

        public async Task<IEnumerable<ProjectSessionDTO>> GetAll(Guid userId)
        {
            var projects = await _projectsRepo.GetAllAsync(userId);
            var timeSessions = await _timeSessionsRepository.GetAllAsync(userId);

            var timeSessionsPerProject = timeSessions.ToLookup(t => t.ProjectId);

            var projectsDtos = projects.Select(p =>
            {
                var isRunning = timeSessionsPerProject[p.Id].Any(ts => ts.EndDate is null);
                var timeSpentTotal = timeSessionsPerProject[p.Id]
                    .Sum(ts => ((ts.EndDate ?? DateTime.UtcNow) - ts.StartTime).TotalSeconds);

                return new ProjectSessionDTO()
                {
                    Id = p.Id,
                    Name = p.Name,
                    Color = p.Color,
                    IsRunning = isRunning,
                    TimeSpentTotal = Math.Round(timeSpentTotal),
                    TimeSpentCurrentSession = isRunning ? Math.Round((DateTime.UtcNow - timeSessionsPerProject[p.Id].First(ts => ts.EndDate is null).StartTime).TotalSeconds) : 0,
                };
            });

            return projectsDtos;
        }
    }
}
