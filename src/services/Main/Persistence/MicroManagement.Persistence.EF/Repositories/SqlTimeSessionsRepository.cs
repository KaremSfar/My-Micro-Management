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
                EndTime = timeSession.EndTime,
                UserId = timeSession.UserId,
            };

            var project = await _dbContext
                .Projects
                .SingleOrDefaultAsync(p => timeSession.ProjectId == p.Id);

            timeSessionEntity.Project = project;

            await _dbSet.AddAsync(timeSessionEntity);
            await _dbContext.SaveChangesAsync();
        }

        public async Task<IEnumerable<TimeSession>> GetAllAsync(Guid userId)
        {
            var timeSessionEntities = (await _dbSet
                .Include(t => t.Project)
                .ToListAsync())
                .Where(t => t.UserId.ToString() == userId.ToString());

            var timesSessions = new List<TimeSession>();

            foreach (var timeSessionEntity in timeSessionEntities)
            {
                var timeSession = timeSessionEntity.Adapt<TimeSession>();
                timeSession.ProjectId = timeSessionEntity.Project.Id;
                timesSessions.Add(timeSession);
            }

            return timesSessions;
        }

        public async Task UpdateAsync(TimeSession timeSession)
        {
            var timeSessionEntity = await _dbSet
                .Where(t => t.UserId.ToString() == timeSession.UserId.ToString())
                .Where(t => t.StartTime == timeSession.StartTime)
                .Include(t => t.Project)
                .FirstOrDefaultAsync();

            if (timeSessionEntity is null)
                return;

            timeSessionEntity.EndTime = timeSession.EndTime;

            _dbSet.Update(timeSessionEntity);

            await _dbContext.SaveChangesAsync();
        }
    }
}
