using MicroManagement.Services.Abstraction;
using MicroManagement.Services.Abstraction.DTOs;

namespace MicroManagement.Services.Mock
{
    public class MockProjectsService : IProjectsService
    {
        private List<ProjectDTO> _projects = new List<ProjectDTO>();
        public Task<ProjectDTO> AddProject(ProjectDTO projectToAdd)
        {
            _projects.Add(projectToAdd);
            return Task.FromResult(projectToAdd);
        }

        public Task<IEnumerable<ProjectDTO>> GetAll()
        {
            return Task.FromResult(_projects as IEnumerable<ProjectDTO>);
        }
    }
}