using MicroManagement.Services.Abstraction.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MicroManagement.Services.Abstraction
{
    public interface ITimeSessionsService
    {
        Task<TimeSessionDTO> StartTimeSession(Guid userId, Guid projectId);
        Task StopTimeSession(Guid userId);
        Task<IEnumerable<TimeSessionDTO>> GetAll(Guid userId);
    }
}
