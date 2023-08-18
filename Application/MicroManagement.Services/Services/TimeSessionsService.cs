using MicroManagement.Application.Services.Abstraction;
using MicroManagement.Core;
using MicroManagement.Persistence.Abstraction.Repositories;
using MicroManagement.Services.Abstraction.DTOs;
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
            return timeSessionDTO;
        }

        public async Task<IEnumerable<TimeSessionDTO>> GetAll()
        {
            return Enumerable.Empty<TimeSessionDTO>();
        }
    }
}
