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
        Task<TimeSessionDTO> AddTimeSession(Guid UserId, TimeSessionDTO timeSessionDTO);
        Task<IEnumerable<TimeSessionDTO>> GetAll(Guid UserId);
    }
}
