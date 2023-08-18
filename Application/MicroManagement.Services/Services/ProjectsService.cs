using MicroManagement.Application.Services.Abstraction;
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

namespace MicroManagement.Application.Services
{
    public class ProjectsService : IProjectsService
    {
        public ProjectsService()
        {
        }

        public async Task<ProjectDTO> AddProject(ProjectDTO addProjectDto)
        {
            return addProjectDto;
        }

        public async Task<IEnumerable<ProjectDTO>> GetAll()
        {
            return Enumerable.Empty<ProjectDTO>();
        }
    }
}
