
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Hosting;

namespace MicroManagement.Shared
{
    public class MigrationsService<T> : IHostedService
        where T : DbContext
    {
        private readonly IDbContextFactory<T> _dbContextFactory;

        public MigrationsService(IDbContextFactory<T> dbContextFactory)
        {
            _dbContextFactory = dbContextFactory;
        }

        public async Task StartAsync(CancellationToken cancellationToken)
        {
            var dbContext = await _dbContextFactory.CreateDbContextAsync();
            await dbContext.Database.MigrateAsync(cancellationToken);
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            return Task.CompletedTask;
        }
    }
}
