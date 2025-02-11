using MicroManagement.Auth.WebAPI.Persistence;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace MicroManagement.Auth.WebAPI
{
    public static class DbInitializationExtensions
    {
        public static IServiceCollection AddAuthDbContext(this IServiceCollection services, IConfiguration configuration)
        {
            string databaseType = configuration["DatabaseType"]!;

            return databaseType switch
            {
                "postgres" => AddPostgresSqlDbContext(services),
                "sqlite" => AddSqliteDbContext(services),
                _ => throw new ArgumentException("Choose Database type in configuration")
            };
        }

        private static IServiceCollection AddSqliteDbContext(IServiceCollection services)
        {
            Console.WriteLine("Hello from sad Karem !");

            services.AddHostedService<SqliteInitializationService>();

            services.AddDbContextFactory<AuthenticationServiceDbContext>(options =>
            {
                var projectRoot = AppDomain.CurrentDomain.BaseDirectory;
                var dbPath = Path.Combine(projectRoot, "auth.db");

                options.UseSqlite($"DataSource={dbPath}", o => o.MigrationsAssembly("MicroManagement.Auth.Migrations.SQLite"));
            });

            return services;
        }

        private static IServiceCollection AddPostgresSqlDbContext(IServiceCollection services)
        {
            Console.WriteLine("Hello from Karem !");
            return services.AddDbContextFactory<AuthenticationServiceDbContext>(options =>
            {
                string connectionString = "Host=localhost;Port=5432;Database=Auth;Username=karem;Password=Neo1234567";

                options.UseNpgsql(connectionString,
                    pgOptions => pgOptions.MigrationsAssembly("MicroManagement.Auth.Migrations.Postgres"));
            });
        }
    }
}
