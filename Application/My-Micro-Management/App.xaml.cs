namespace My_Micro_Management;

public partial class App : Application
{
    public App()
    {
        InitializeComponent();
        MainPage = new AppShell();
    }

    protected override Window CreateWindow(IActivationState activationState)
    {
        var window = base.CreateWindow(activationState);
#if WINDOWS
        const int newWidth = 1000;
        const int newHeight = 600;

        window.Width = newWidth;
        window.Height = newHeight;
#endif
        return window;
    }

}
