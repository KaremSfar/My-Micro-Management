using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace MicroManagement.Shared
{
    public static class DbInitializationExtensions
    {
        public static IServiceCollection AddDbContext<T, S>(this IServiceCollection services, IConfiguration configuration)
            where T : DbContext
        {
            var dbSettings = configuration.GetSection(DatabaseSettings.SectionName).Get<DatabaseSettings>()!;

            return dbSettings.DatabaseType switch
            {
                "postgres" => AddPostgresSqlDbContext<T, S>(services, dbSettings),
                "sqlite" => AddSqliteDbContext<T>(services),
                _ => throw new ArgumentException("Choose Database type in configuration")
            };
        }

        private static IServiceCollection AddSqliteDbContext<T>(IServiceCollection services)
            where T : DbContext
        {
            services.AddHostedService<SqliteInitializationService<T>>();

            services.AddDbContextFactory<T>(options =>
            {
                var projectRoot = AppDomain.CurrentDomain.BaseDirectory;
                var dbPath = Path.Combine(projectRoot, "auth.db");

                options.UseSqlite($"DataSource={dbPath}", o => o.MigrationsAssembly("MicroManagement.Auth.Migrations.SQLite"));
            });
            return services;
        }

        private static IServiceCollection AddPostgresSqlDbContext<T, S>(IServiceCollection services, DatabaseSettings settings)
            where T : DbContext
        {
            services.AddHostedService<MigrationsService<T>>();

            return services.AddDbContextFactory<T>(options =>
            {
                options.UseNpgsql(settings.ConnectionString,
                    pgOptions => pgOptions.MigrationsAssembly(typeof(S).Assembly.GetName().Name));
            });
        }
    }
}
