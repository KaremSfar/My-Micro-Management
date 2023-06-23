using MicroManagement.Persistence.Abstraction.Repositories;
using MicroManagement.Persistence.SQLite.Configuration;
using MicroManagement.Persistence.SQLite.Repositories;
using MicroManagement.Services;
using MicroManagement.Services.Abstraction;
using MicroManagement.Services.Abstraction.Services;
using MicroManagement.Services.Mock;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace My_Micro_Management;

public static class MauiProgram
{

    private static IServiceProvider serviceProvider;

    public static TService GetService<TService>()
        => serviceProvider.GetService<TService>();

    public static MauiApp CreateMauiApp()
    {
        var builder = MauiApp.CreateBuilder();
        builder
            .UseMauiApp<App>()
            .ConfigureFonts(fonts =>
            {
                fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
            });

#if DEBUG
        builder.Logging.AddDebug();
#endif
        // TODO-KAREM: this is now here as we're option for a Local SQLite DB, 
        // when, and if, we're getting this project to a client-service app, remove these
        builder.Services.AddDbContext<MyMicroManagementDbContext>(options =>
        {
            options.UseSqlite(@"Data Source=C:\Repos\Temp\MyDB-dev.db");
        });

        builder.Services.AddSingleton<IProjectsRepository, SQLiteProjectsRepository>();
        builder.Services.AddSingleton<ITimeSessionsRepository, SQLiteTimeSessionsRepository>();

        builder.Services.AddSingleton<ITimeSessionsService, TimeSessionsService>();
        builder.Services.AddSingleton<IProjectsService, ProjectsService>();
        builder.Services.AddSingleton<ITimeSessionsExporter, TimeSessionExporter>();

        var app = builder.Build();
        serviceProvider = app.Services;

        return app;
    }
}
