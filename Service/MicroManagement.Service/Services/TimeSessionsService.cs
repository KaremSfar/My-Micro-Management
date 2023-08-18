using MicroManagement.Core;
using MicroManagement.Persistence.Abstraction.Repositories;
using MicroManagement.Services.Abstraction;
using MicroManagement.Services.Abstraction.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MicroManagement.Services
{
    public class TimeSessionsService : ITimeSessionsService
    {
        private ITimeSessionsRepository _timeSessionsRepo;

        public TimeSessionsService(ITimeSessionsRepository timeSessionsRepo)
        {
            _timeSessionsRepo = timeSessionsRepo;
        }

        public async Task<TimeSessionDTO> AddTimeSession(TimeSessionDTO timeSessionDTO)
        {
            var timeSession = new TimeSession()
            {
                StartTime = timeSessionDTO.StartTime,
                EndDate = timeSessionDTO.EndDate,
                ProjectIds = timeSessionDTO.ProjectIds,
            };

            await _timeSessionsRepo.AddAsync(timeSession);
            return timeSessionDTO;
        }

        public async Task<IEnumerable<TimeSessionDTO>> GetAll()
        {
            return (await _timeSessionsRepo.GetAllAsync()).Select(ts => new TimeSessionDTO() { StartTime = ts.StartTime, EndDate = ts.EndDate, ProjectIds = ts.ProjectIds });

        }
    }
}
