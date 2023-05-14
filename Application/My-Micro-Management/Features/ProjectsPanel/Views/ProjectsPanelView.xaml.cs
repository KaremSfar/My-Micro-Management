using MicroManagement.Services.Abstraction.DTOs;
using My_Micro_Management.Features.Timer;
using Windows.UI.Popups;

namespace My_Micro_Management.Features.ProjectsPanel;

public partial class ProjectsPanelView : ContentView
{
    public ProjectsListViewModel ProjectsViewModel { get; set; } = new ProjectsListViewModel();
    
    public ProjectsPanelView()
    {
        InitializeComponent();
        this.BindingContext = ProjectsViewModel;
    }

    private async void AddProject_Clicked(object sender, EventArgs e)
    {
        var addProjectView = new AddProjectView();

        var window = new Window(addProjectView);
        window.Height = 300;
        window.Width = 400;

        Application.Current.OpenWindow(window);

        addProjectView.ProjectAdded += async (_, _) =>
        {
            Application.Current.CloseWindow(window);
            await ProjectsViewModel.FetchProjects();
        };
    }
}