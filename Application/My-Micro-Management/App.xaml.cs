using My_Micro_Management.Features.Auth;

namespace My_Micro_Management;

public partial class App : Application
{
    public App()
    {
        InitializeComponent();
        MainPage = new AuthPage();
    }

    protected override Window CreateWindow(IActivationState activationState)
    {
        var window = base.CreateWindow(activationState);
#if WINDOWS
        window.Width = 1000;
        window.Height = 630;
#endif
        return window;
    }

}
