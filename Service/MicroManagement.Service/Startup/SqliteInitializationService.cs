
using MicroManagement.Persistence.EF.Configuration;
using Microsoft.EntityFrameworkCore;

namespace MicroManagement.Auth.WebAPI
{
    /// <summary>
    /// Initializes the SQLite DB in case it's not
    /// </summary>
    public class SqliteInitializationService(IDbContextFactory<MyMicroManagementDbContext> _dbContextFactory) : IHostedService
    {
        /// <inheritdoc/>
        public async Task StartAsync(CancellationToken cancellationToken)
        {
            using var dbContext = await _dbContextFactory.CreateDbContextAsync();

            if (!dbContext.Database.IsSqlite())
                throw new InvalidOperationException("WARNING, This service should only be injected for Local Development on SQLite DB!");

            await dbContext.Database.EnsureCreatedAsync();
        }

        /// <inheritdoc/>
        public Task StopAsync(CancellationToken cancellationToken)
            => Task.CompletedTask;

    }
}
