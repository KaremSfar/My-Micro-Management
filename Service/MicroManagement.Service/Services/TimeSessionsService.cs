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

        public async Task<TimeSessionDTO> StartTimeSession(Guid userId, Guid projectId)
        {
            var timeSession = new TimeSession()
            {
                UserId = userId,
                StartTime = DateTime.UtcNow,
                EndDate = null,
                ProjectIds = new List<Guid> { projectId },
            };

            await _timeSessionsRepo.AddAsync(timeSession);

            return new TimeSessionDTO
            {
                StartTime = timeSession.StartTime
            };
        }

        public async Task<IEnumerable<TimeSessionDTO>> GetAll(Guid userId)
        {
            return (await _timeSessionsRepo.GetAllAsync(userId))
                .Select(ts => new TimeSessionDTO() { StartTime = ts.StartTime, EndDate = ts.EndDate, ProjectIds = ts.ProjectIds });
        }

        public async Task StopTimeSession(Guid userId)
        {
            var timeSessions = (await _timeSessionsRepo.GetAllAsync(userId))
                .Where(ts => ts.EndDate is null)
                .ToList();

            if (timeSessions.Count > 1)
                Console.WriteLine("HAAAAA some time sessions were started while others not stopped");

            var timeSessionsToStop = timeSessions.Select(ts => ts with
            {
                EndDate = DateTime.UtcNow
            }).ToList();

            foreach (var timeSessionToStop in timeSessionsToStop)
            {
                await _timeSessionsRepo.UpdateAsync(timeSessionToStop);
            }
        }
    }
}
