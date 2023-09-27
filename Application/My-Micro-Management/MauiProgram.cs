using CommunityToolkit.Maui;
using MicroManagement.Application.Common;
using MicroManagement.Application.Services;
using MicroManagement.Application.Services.Abstraction;
using MicroManagement.Application.Services.Abstractions;
using MicroManagement.Application.Services.Services;
using MicroManagement.Services.Abstraction;
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
            .UseMauiCommunityToolkit()
            .ConfigureFonts(fonts =>
            {
                fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
            });

#if DEBUG
        builder.Logging.AddDebug();
#endif

        builder.Services.AddSingleton<ITimeSessionsService, TimeSessionsService>();
        builder.Services.AddSingleton<IProjectsService, ProjectsService>();
        builder.Services.AddSingleton<ITimeSessionsExporter, TimeSessionExporter>();
        builder.Services.AddSingleton<IAuthenticationService, AuthenticationService>();
        builder.Services.AddSingleton<IAuthenticationContextProvider, AuthenticationContextProvider>();

        var app = builder.Build();
        serviceProvider = app.Services;

        return app;
    }
}
