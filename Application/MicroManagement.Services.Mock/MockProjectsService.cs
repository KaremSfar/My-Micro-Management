using MicroManagement.Application.Services.Abstraction;
using MicroManagement.Services.Abstraction.DTOs;

namespace MicroManagement.Services.Mock
{
    public class MockProjectsService : IProjectsService
    {
        private List<ProjectDTO> _projects = new List<ProjectDTO>()
        {
            new ProjectDTO() {Id = Guid.NewGuid(), Name = "Sprint", Color = "#e67e22"},
            new ProjectDTO() {Id = Guid.NewGuid(), Name = "Pause", Color = "#2ecc71"},
            new ProjectDTO() {Id = Guid.NewGuid(), Name = "Bugs", Color = "#c0392b"},
            new ProjectDTO() {Id = Guid.NewGuid(), Name = "External", Color = "#9b59b6"},
            new ProjectDTO() {Id = Guid.NewGuid(), Name = "Internal", Color = "#3498db"},
            new ProjectDTO() {Id = Guid.NewGuid(), Name = "Calls", Color = "#34495e"},
        };

        public Task<ProjectDTO> AddProject(ProjectDTO projectToAdd)
        {
            projectToAdd.Id = Guid.NewGuid();
            _projects.Add(projectToAdd);
            return Task.FromResult(projectToAdd);
        }

        public Task<IEnumerable<ProjectDTO>> GetAll()
        {
            return Task.FromResult(_projects as IEnumerable<ProjectDTO>);
        }
    }
}