﻿// See https://aka.ms/new-console-template for more information
using MicroManagement.Auth.WebAPI.Persistence;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

Console.WriteLine("Hello, World!");

// This host is used to trick EF Migration Tool to use this project as migration applier
using IHost host = Host.CreateDefaultBuilder(args)
    .ConfigureServices(services =>
    {
        services.AddDbContext<AuthenticationServiceDbContext>(options =>
        {
            options.UseSqlite(
                connectionString: @"Data Source=C:\Repos\Temp\MyAuthDB-dev.db", // FIXME: better connection string and multiple DBs
                sqliteOptionsAction: b => b.MigrationsAssembly("MicroManagement.Auth.Migrations.SQLite"));
        });
    })
    .Build();
