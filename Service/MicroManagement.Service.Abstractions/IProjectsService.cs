﻿using MicroManagement.Services.Abstraction.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MicroManagement.Services.Abstraction
{
    public interface IProjectsService
    {
        Task<IEnumerable<ProjectSessionDTO>> GetAll(Guid userId);
        Task<GetProjectDTO> AddProject(Guid userId, CreateProjectDTO projectToAdd);
    }
}
