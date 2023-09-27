// See https://aka.ms/new-console-template for more information
using MicroManagement.Persistence.EF.Configuration;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;


// This host is used to trick EF Migration Tool to use this project as migration applier
using IHost host = Host.CreateDefaultBuilder(args)
    .ConfigureServices(services =>
    {
        services.AddDbContext<MyMicroManagementDbContext>(options =>
        {
            options.UseSqlServer(
                connectionString: "ConnectionString",
                sqlServerOptionsAction: (b) => b.MigrationsAssembly("MicroManagement.Persistence.Migrations.AzureSQL"));
        });
    })
    .Build();
