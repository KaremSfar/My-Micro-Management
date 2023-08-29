using MicroManagement.Persistence.EF.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MicroManagement.Persistence.EF.Configuration
{
    public class MyMicroManagementDbContext : DbContext
    {
        public MyMicroManagementDbContext() { }
        public MyMicroManagementDbContext(DbContextOptions options) : base(options) { }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
            => optionsBuilder.LogTo(Console.WriteLine);

        public DbSet<ProjectEntity> Projects { get; set; }
        public DbSet<TimeSessionEntity> TimeSessions { get; set; }
    }
}
