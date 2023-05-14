using MicroManagement.Services.Abstraction.DTOs;
using MicroManagement.Services.Abstraction.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MicroManagement.Services.Mock
{
    public class MockTimeSessionsService : ITimeSessionsService
    {
        private List<TimeSessionDTO> _sessions = new List<TimeSessionDTO>();
        public async Task<TimeSessionDTO> AddTimeSession(TimeSessionDTO timeSessionDTO)
        {
            _sessions.Add(timeSessionDTO);
            return timeSessionDTO;
        }
    }
}
