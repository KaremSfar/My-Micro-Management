using MicroManagement.Persistence.Abstraction.Repositories;
using MicroManagement.Persistence.SQLite.Configuration;
using MicroManagement.Persistence.SQLite.Repositories;
using MicroManagement.Services;
using MicroManagement.Services.Abstraction;
using MicroManagement.Services.Abstraction.Services;
using MicroManagement.Services.Mock;
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

        builder.Services.AddDbContext<SQLiteDbContext>();

        builder.Services.AddSingleton<IProjectsRepository, SQLiteProjectsRepository>();

        builder.Services.AddSingleton<ITimeSessionsService, MockTimeSessionsService>();
        builder.Services.AddSingleton<IProjectsService, ProjectsService>();
        builder.Services.AddSingleton<ITimeSessionsExporter, TimeSessionExporter>();

        var app = builder.Build();
        serviceProvider = app.Services;

        return app;
    }
}
