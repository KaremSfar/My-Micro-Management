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
        public SQLiteDbContext(DbContextOptions options) : base(options) { }

        public DbSet<ProjectEntity> Projects { get; set; }
    }
}
