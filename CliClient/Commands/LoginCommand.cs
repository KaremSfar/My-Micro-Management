using CliClient.States;

namespace CliClient.Commands;

public class LoginCommandHandler : ICommandHandler
{
    private readonly IAuthService _authService;

    public string Email { get; }
    public string Password { get; }

    public LoginCommandHandler(IAuthService loginService, string email, string password)
    {
        _authService = loginService;
        this.Email = email;
        this.Password = password;
    }

    public async Task<IState> Execute()
    {
        await _authService.LoginAsync(Email, Password);
        return new LoginState(_authService);
    }
}