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

        public ProjectsService(IProjectsRepository projectsRepo)
        {
            _projectsRepo = projectsRepo;
        }

        public async Task<ProjectDTO> AddProject(Guid userId, ProjectDTO addProjectDto)
        {
            var projectToAdd = new Project(Guid.NewGuid(), userId, addProjectDto.Name, addProjectDto.Color);

            await _projectsRepo.AddProjectAsync(projectToAdd);
            return addProjectDto;
        }

        public async Task<IEnumerable<ProjectDTO>> GetAll(Guid userId)
        {
            var projects = await _projectsRepo.GetAllAsync(userId);
            return projects.Select(t => new ProjectDTO() { Id = t.Id, Name = t.Name, Color = t.Color });
        }
    }
}
