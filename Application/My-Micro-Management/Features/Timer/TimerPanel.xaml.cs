using My_Micro_Management.Features.Timer.ViewModels;

namespace My_Micro_Management.Features.Timer;

public partial class TimerPanel : ContentView
{
    public TimerViewModel ViewModel { get; set; }
    public TimerPanel()
    {
        InitializeComponent();
        this.BindingContext = ViewModel;
    }
}