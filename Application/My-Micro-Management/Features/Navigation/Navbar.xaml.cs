using MicroManagement.Services;
using MicroManagement.Services.Abstraction.Services;
using My_Micro_Management.Features.ProjectsPanel;
using My_Micro_Management.Features.Timer;
using System;

namespace My_Micro_Management.Features.Navigation;

public partial class Navbar : ContentView
{
    private ITimeSessionsExporter _timeSessionExporter = new TimeSessionExporter(
        TimerServicesFactory.Instance,
        ProjectsServiceFactory.Instance);

    public Navbar()
    {
        InitializeComponent();
    }

    private async void Button_Clicked(object sender, EventArgs e)
    {
        var content = await _timeSessionExporter.ExportTimeSession();

        string targetFile = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData),
            "MicroMgmt",
            $"Sessions - {DateTime.Now.DayOfWeek}.csv");

        await File.AppendAllTextAsync(targetFile, content);
    }
}