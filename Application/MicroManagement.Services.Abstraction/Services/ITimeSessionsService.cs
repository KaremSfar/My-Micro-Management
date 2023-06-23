﻿using MicroManagement.Services.Abstraction.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MicroManagement.Application.Services.Abstraction
{
    public interface ITimeSessionsService
    {
        Task<TimeSessionDTO> AddTimeSession(TimeSessionDTO timeSessionDTO);
        Task<IEnumerable<TimeSessionDTO>> GetAll();
    }
}
