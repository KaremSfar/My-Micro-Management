using MicroManagement.Persistence.SQLite.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MicroManagement.Persistence.SQLite.Configuration
{
    public class SQLiteDbContext : DbContext
    {
        public SQLiteDbContext() { }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlite(@"Data Source=C:\Repos\Temp\MyDB.db");
        }

        public DbSet<ProjectEntity> Projects { get; set; }
    }
}
