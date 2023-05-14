using MicroManagement.Services.Abstraction.DTOs;
using Microsoft.UI.Xaml;
using My_Micro_Management.Features.Timer.ViewModels;

namespace My_Micro_Management.Features.Timer;

public partial class TimerPanel : ContentView
{
    public ProjectDTO SelectedProject
    {
        get { return (ProjectDTO)GetValue(SelectedProjectProperty); }
        set { SetValue(SelectedProjectProperty, value); }
    }

    public static readonly BindableProperty SelectedProjectProperty = BindableProperty.CreateAttached(
        "SelectedProject",
        typeof(ProjectDTO),
        typeof(TimerViewModel),
        null,
        propertyChanged: (bindable, oldValue, newValue) =>
        {
            (bindable as TimerPanel).ViewModel.SelectedProject = newValue as ProjectDTO;
        });

    public TimerViewModel ViewModel { get; set; } = new TimerViewModel();
    public TimerPanel()
    {
        InitializeComponent();
        this.BindingContext = ViewModel;
    }
}