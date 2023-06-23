using MicroManagement.Persistence.SQLite.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MicroManagement.Persistence.SQLite.Configuration
{
    public class MyMicroManagementDbContext : DbContext
    {
        public MyMicroManagementDbContext() { }
        public MyMicroManagementDbContext(DbContextOptions options) : base(options) { }

        public DbSet<ProjectEntity> Projects { get; set; }
        public DbSet<TimeSessionEntity> TimeSessions { get; set; }
    }
}
