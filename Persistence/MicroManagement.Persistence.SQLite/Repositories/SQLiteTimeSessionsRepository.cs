﻿using Mapster;
using MicroManagement.Core;
using MicroManagement.Persistence.Abstraction.Repositories;
using MicroManagement.Persistence.SQLite.Configuration;
using MicroManagement.Persistence.SQLite.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MicroManagement.Persistence.SQLite.Repositories
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
            var timeSessions = await _dbSet
                .Select(ts => ts.Adapt<TimeSession>())
                .ToListAsync();
            return timeSessions;
        }
    }
}
