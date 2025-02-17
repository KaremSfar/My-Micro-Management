using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace MicroManagement.Shared;

public static class DbInitializationExtensions
{
    public static IServiceCollection AddDatabaseContext<T>(this IServiceCollection services, DatabaseSettings dbConfigurationSettings, Action<DbSetupOptions> optionsAction = null)
        where T : DbContext
    {
        DbSetupOptions dbOptions = new();

        optionsAction?.Invoke(dbOptions);

        return dbConfigurationSettings.DatabaseType switch
        {
            "postgres" => AddPostgresSqlDbContext<T>(services, dbConfigurationSettings, dbOptions),
            "sqlite" => AddSqliteDbContext<T>(services, dbConfigurationSettings, dbOptions),
            _ => throw new ArgumentException("Choose Database type in configuration")
        };
    }

    private static IServiceCollection AddSqliteDbContext<T>(IServiceCollection services, DatabaseSettings settings, DbSetupOptions dbOptions)
        where T : DbContext
    {
        services.AddHostedService<MigrationsService<T>>();

        services.AddDbContextFactory<T>(options =>
        {
            var projectRoot = AppDomain.CurrentDomain.BaseDirectory;
            var dbPath = Path.Combine(projectRoot, settings.ConnectionString);

            options.UseSqlite($"DataSource={dbPath}", o => o.MigrationsAssembly(dbOptions?.MigrationAssembly));
        });
        return services;
    }

    private static IServiceCollection AddPostgresSqlDbContext<T>(IServiceCollection services, DatabaseSettings settings, DbSetupOptions dbOptions)
        where T : DbContext
    {
        services.AddHostedService<MigrationsService<T>>();

        return services.AddDbContextFactory<T>(options =>
        {
            options.UseNpgsql(settings.ConnectionString,
                pgOptions => pgOptions.MigrationsAssembly(dbOptions?.MigrationAssembly));
        });
    }
}

public class DbSetupOptions
{
    public string MigrationAssembly { get; set; }

    public void MigrationsAssembly(string assembly)
    {
        this.MigrationAssembly = assembly;
    }
}
