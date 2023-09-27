using MicroManagement.Application.Common;
using MicroManagement.Application.Services.Abstractions;
using My_Micro_Management.Features.Auth;

namespace My_Micro_Management;

public partial class App : Application
{
    private readonly IAuthenticationContextProvider _authenticationContextProvider = MauiProgram.GetService<IAuthenticationContextProvider>();

    public App()
    {
        this.MainPage = new SplashScreen();
        InitializeAsync();
    }

    public async Task InitializeAsync()
    {
        if (await _authenticationContextProvider.IsAuthenticated())
        {
            await this._authenticationContextProvider.RefreshTokens();
            this._authenticationContextProvider.StartTokensAutoRefresh();
            MainPage = new AppShell();
        }
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
