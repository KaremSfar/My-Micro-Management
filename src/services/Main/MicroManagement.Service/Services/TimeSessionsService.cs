using MicroManagement.Core;
using MicroManagement.Persistence.Abstraction.Repositories;
using MicroManagement.Service.WebAPI.Events;
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
        private ITimeSessionEventsPublisher _timeSessionEventsPublisher;

        public TimeSessionsService(ITimeSessionsRepository timeSessionsRepo, ITimeSessionEventsPublisher timeSessionEventsPublisher)
        {
            _timeSessionsRepo = timeSessionsRepo;
            _timeSessionEventsPublisher = timeSessionEventsPublisher;
        }

        public async Task<IEnumerable<TimeSessionDTO>> GetAll(Guid userId)
        {
            return (await _timeSessionsRepo.GetAllAsync(userId))
                .Select(ts => new TimeSessionDTO() { StartTime = ts.StartTime, EndTime = ts.EndTime, ProjectId = ts.ProjectId });
        }

        public async Task<TimeSessionDTO> StartTimeSession(Guid userId, Guid projectId)
        {
            await StopTimeSession(userId);

            var timeSession = new TimeSession()
            {
                UserId = userId,
                StartTime = DateTime.UtcNow,
                EndTime = null,
                ProjectId = projectId,
            };

            await _timeSessionsRepo.AddAsync(timeSession);

            await _timeSessionEventsPublisher.PublishTimeSessionStartedEventAsync(userId, projectId);

            return new TimeSessionDTO
            {
                StartTime = timeSession.StartTime,
                ProjectId = projectId,
            };
        }

        public async Task StopTimeSession(Guid userId)
        {
            var timeSessions = (await _timeSessionsRepo.GetAllAsync(userId))
                .Where(ts => ts.EndTime is null)
                .ToList();

            if (!timeSessions.Any())
                return;

            if (timeSessions.Count > 1)
                Console.WriteLine("HAAAAA some time sessions were started while others not stopped");

            var timeSessionsToStop = timeSessions.Select(ts => ts with
            {
                EndTime = DateTime.UtcNow
            }).ToList();

            foreach (var timeSessionToStop in timeSessionsToStop)
            {
                await _timeSessionsRepo.UpdateAsync(timeSessionToStop);
            }

            await _timeSessionEventsPublisher.PublishTimeSessionStoppedEventAsync(userId);
        }
    }
}
