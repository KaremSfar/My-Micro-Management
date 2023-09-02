using MicroManagement.Application.Common;
using My_Micro_Management.Features.Auth;

namespace My_Micro_Management;

public partial class App : Application
{
    private readonly IAuthenticationContextProvider _authenticationContextProvider = MauiProgram.GetService<IAuthenticationContextProvider>();

    public App()
    {
        InitializeComponent();
        if (_authenticationContextProvider.IsAuthenticated().Result)
            MainPage = new AppShell();
        else
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
