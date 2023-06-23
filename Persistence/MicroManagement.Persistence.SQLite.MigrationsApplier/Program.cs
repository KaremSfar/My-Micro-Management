// See https://aka.ms/new-console-template for more information
using MicroManagement.Persistence.EF.Configuration;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

Console.WriteLine("Hello, World!");

// This host is used to trick EF Migration Tool to use this project as migration applier
using IHost host = Host.CreateDefaultBuilder(args)
    .ConfigureServices(services =>
    {
        services.AddDbContext<MyMicroManagementDbContext>(options =>
        {
            options.UseSqlite(
                connectionString: @"Data Source=C:\Repos\Temp\MyDB-dev.db", // FIXME: better connection string and multiple DBs
                sqliteOptionsAction: b => b.MigrationsAssembly("MicroManagement.Persistence.SQLite.MigrationsApplier"));
        });
    })
    .Build();
