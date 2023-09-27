using MicroManagement.Application.Common;
using MicroManagement.Application.Services.Abstraction;
using MicroManagement.Application.Services.Abstractions;
using MicroManagement.Services;
using MicroManagement.Services.Abstraction;
using My_Micro_Management.Features.Auth;
using My_Micro_Management.Features.ProjectsPanel;
using My_Micro_Management.Features.Timer;
using System;

namespace My_Micro_Management.Features.Navigation;

public partial class Navbar : ContentView
{
    private readonly ITimeSessionsExporter _timeSessionExporter = MauiProgram.GetService<ITimeSessionsExporter>();
    private readonly IAuthenticationContextProvider _authenticationContextProvider = MauiProgram.GetService<IAuthenticationContextProvider>();

    public Navbar()
    {
        InitializeComponent();
    }

    private async void Button_Clicked(object sender, EventArgs e)
    {
        var content = await _timeSessionExporter.ExportTimeSession();

        string targetFile = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData),
            "MicroMgmt",
            $"Sessions - {DateTime.Now.DayOfWeek} - {DateTime.Now.Day}.csv");

        await File.AppendAllTextAsync(targetFile, content);
    }

    private void SignOutBtn_Clicked(object sender, EventArgs e)
    {
        this._authenticationContextProvider.SignOut();
        Application.Current.MainPage = new AuthPage();
    }
}