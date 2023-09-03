namespace My_Micro_Management.Features.Auth;

public partial class AuthPage : ContentPage
{
    private readonly AuthViewModel _authViewModel = new AuthViewModel();
    public AuthPage()
    {
        InitializeComponent();
        BindingContext = _authViewModel;
    }

    private async void LoginBtn_Clicked(object sender, EventArgs e)
    {
        await this._authViewModel.Login();
        Application.Current.MainPage = new AppShell();
    }

    private async void SignupButton_Clicked(object sender, EventArgs e)
    {
        await this._authViewModel.Signup();
        Application.Current.MainPage = new AppShell();
    }

    private void SignupView_Tapped(object sender, TappedEventArgs e)
    {
        _authViewModel.IsSignup = !_authViewModel.IsSignup;
    }
}