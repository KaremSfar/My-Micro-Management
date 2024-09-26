
using MicroManagement.Auth.WebAPI.Persistence;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;

namespace MicroManagement.Auth.WebAPI
{
    /// <summary>
    /// Initializes the SQLite DB in case it's not
    /// </summary>
    public class SqliteInitializationService(IDbContextFactory<AuthenticationServiceDbContext> _dbContextFactory) : IHostedService
    {
        /// <inheritdoc/>
        public async Task StartAsync(CancellationToken cancellationToken)
        {
            using var dbContext = await _dbContextFactory.CreateDbContextAsync();

            if (!dbContext.Database.IsSqlite())
                throw new InvalidOperationException("WARNING, This service should only be injected for Local Development on SQLite DB!");

            var dbPath = GetSqliteDatabaseFilePath(dbContext);

            if (!File.Exists(dbPath))
            {
                // Create the empty database file to allow migrations to run
                File.Create(dbPath).Dispose();
            }

            await dbContext.Database.EnsureCreatedAsync();
        }

        /// <inheritdoc/>
        public Task StopAsync(CancellationToken cancellationToken)
            => Task.CompletedTask;


        private string GetSqliteDatabaseFilePath(DbContext dbContext)
        {
            var connectionString = dbContext.Database.GetConnectionString();

            var builder = new SqliteConnectionStringBuilder(connectionString);
            return builder.DataSource;
        }
    }
}
