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
    public class SQLiteTimeSessionsRepository : ITimeSessionsRepository
    {
        private MyMicroManagementDbContext _dbContext;
        private DbSet<TimeSessionEntity> _dbSet => _dbContext.TimeSessions;

        public SQLiteTimeSessionsRepository(MyMicroManagementDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task AddAsync(TimeSession timeSession)
        {
            var timeSessionEntity = new TimeSessionEntity()
            {
                StartTime = timeSession.StartTime,
                EndDate = timeSession.EndDate,
            };

            var projects = await _dbContext
                .Projects
                .Where(p => timeSession.ProjectIds.Contains(p.Id))
                .ToListAsync();

            timeSessionEntity.Projects = projects;

            await _dbSet.AddAsync(timeSessionEntity);
            await _dbContext.SaveChangesAsync();
        }

        public async Task<IEnumerable<TimeSession>> GetAllAsync()
        {
            var timeSessionEntities = await _dbSet
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
    }
}
