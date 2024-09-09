using Mapster;
using MicroManagement.Core;
using MicroManagement.Persistence.Abstraction.Repositories;
using MicroManagement.Persistence.EF.Configuration;
using MicroManagement.Persistence.EF.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MicroManagement.Persistence.EF.Repositories
{
    public class SqlTimeSessionsRepository : ITimeSessionsRepository
    {
        private MyMicroManagementDbContext _dbContext;
        private DbSet<TimeSessionEntity> _dbSet => _dbContext.TimeSessions;

        public SqlTimeSessionsRepository(MyMicroManagementDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task AddAsync(TimeSession timeSession)
        {
            var timeSessionEntity = new TimeSessionEntity()
            {
                StartTime = timeSession.StartTime,
                EndDate = timeSession.EndDate,
                UserId = timeSession.UserId,
            };

            var projects = await _dbContext
                .Projects
                .Where(p => timeSession.ProjectIds.Contains(p.Id))
                .ToListAsync();

            timeSessionEntity.Projects = projects;

            await _dbSet.AddAsync(timeSessionEntity);
            await _dbContext.SaveChangesAsync();
        }

        public async Task<IEnumerable<TimeSession>> GetAllAsync(Guid userId)
        {
            var timeSessionEntities = await _dbSet
                .Where(t => t.UserId.ToString() == userId.ToString())
                .Include(t => t.Projects)
                .ToListAsync();

            var timesSessions = new List<TimeSession>();

            foreach (var timeSessionEntity in timeSessionEntities)
            {
                var timeSession = timeSessionEntity.Adapt<TimeSession>();
                timeSession.ProjectIds = timeSessionEntity.Projects.Select(p => p.Id).ToList();
                timesSessions.Add(timeSession);
            }

            return timesSessions;
        }

        public async Task UpdateAsync(TimeSession timeSession)
        {
            var timeSessionEntity = await _dbSet
                .Where(t => t.UserId.ToString() == timeSession.UserId.ToString())
                .Where(t => t.StartTime == timeSession.StartTime)
                .Include(t => t.Projects)
                .FirstOrDefaultAsync();

            if (timeSessionEntity is null)
                return;

            timeSessionEntity.EndDate = timeSession.EndDate;

            _dbSet.Update(timeSessionEntity);

            await _dbContext.SaveChangesAsync();
        }
    }
}
