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
    public class SQLiteProjectsRepository : IProjectsRepository
    {
        private SQLiteDbContext _dbContext;

        public SQLiteProjectsRepository(SQLiteDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task AddProjectAsync(Project project)
        {
            var projectEntity = new ProjectEntity();
            project.Adapt(projectEntity);

            _dbContext.Projects.Add(projectEntity);
            await _dbContext.SaveChangesAsync();
        }

        public async Task DeleteProjectAsync(Guid projectId)
        {
            var project = await _dbContext.Projects.FindAsync(projectId);

            if (project == null)
                throw new KeyNotFoundException(nameof(Project.Id));

            _dbContext.Projects.Remove(project);

            await _dbContext.SaveChangesAsync();
        }

        public async Task<IEnumerable<Project>> GetAllAsync()
        {
            return await _dbContext
                .Projects
                .Select(p => p.Adapt<Project>())
                .ToListAsync();
        }
    }
}