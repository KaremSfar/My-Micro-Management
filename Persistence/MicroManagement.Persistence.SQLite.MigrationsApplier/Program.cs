// See https://aka.ms/new-console-template for more information
using MicroManagement.Persistence.SQLite.Configuration;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

Console.WriteLine("Hello, World!");
using IHost host = Host.CreateDefaultBuilder(args)
    .ConfigureServices(services =>
    {
        services.AddDbContext<SQLiteDbContext>(options =>
        {
            options.UseSqlite(@"Data Source=C:\Repos\Temp\MyDB-dev.db");
        });
    })
    .Build();
