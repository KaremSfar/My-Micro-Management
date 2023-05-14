using MicroManagement.Services.Abstraction;
using MicroManagement.Services.Abstraction.DTOs;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Xml.Linq;

namespace My_Micro_Management.Features.ProjectsPanel;

public partial class AddProjectView : ContentPage
{
    public EventHandler<ProjectDTO> ProjectAdded;
    public AddProjectViewModel ViewModel { get; set; } = new AddProjectViewModel();
    public AddProjectView()
    {
        InitializeComponent();
        BindingContext = ViewModel;
    }
    private async void Button_Clicked(object sender, EventArgs e)
    {
        await this.ViewModel.AddProject();
        ProjectAdded?.Invoke(this, ViewModel.ProjectToAdd);
    }
}

public class AddProjectViewModel : INotifyPropertyChanged
{
    public event PropertyChangedEventHandler PropertyChanged;
    private IProjectsService _projectsService = ProjectsServiceFactory.Instance;

    private ProjectDTO _projectToAdd;
    public ProjectDTO ProjectToAdd
    {
        get { return _projectToAdd; }
        set { _projectToAdd = value; OnPropertyChanged(); }
    }

    public AddProjectViewModel()
    {
        ProjectToAdd = new ProjectDTO();
    }

    public void OnPropertyChanged([CallerMemberName] string propertyName = null)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }

    public async Task AddProject()
    {
        await _projectsService.AddProject(this.ProjectToAdd);
    }
}