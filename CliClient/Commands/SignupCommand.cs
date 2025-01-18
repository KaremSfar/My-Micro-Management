using CliClient.States;

namespace CliClient.Commands;

public class SignupCommand : ICommandHandler
{
    private readonly IAuthService _authService;

    public string Email { get; }
    public string Password { get; }
    public string FirstName { get; }
    public string LastName { get; }

    public SignupCommand(IAuthService loginService, string email, string password, string firstName, string lastName)
    {
        _authService = loginService;
        this.Email = email;
        this.Password = password;
        FirstName = firstName;
        LastName = lastName;
    }

    public async Task<IState> Execute()
    {
        await _authService.SignupAsync(Email, Password, FirstName, LastName);
        return new LoginState(_authService);
    }
}