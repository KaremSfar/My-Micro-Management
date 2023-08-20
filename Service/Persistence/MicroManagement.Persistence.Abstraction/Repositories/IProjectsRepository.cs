using MicroManagement.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MicroManagement.Persistence.Abstraction.Repositories
{
    public interface IProjectsRepository
    {
        Task<IEnumerable<Project>> GetAllAsync(Guid UserId);
        Task AddProjectAsync(Project project);
        Task DeleteProjectAsync(Guid projectId);
    }
}
