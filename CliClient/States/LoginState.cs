using CliClient.Commands;
using Spectre.Console;

namespace CliClient.States;

public class LoginState : IState
{
    private readonly IAuthService _authService;

    private string _email = "";
    private string _password = "";
    private int _selectedIndex = 0;
    private readonly string[] _options = { "Email", "Password", "Go to Signup", "Login" };
    private Table _table;

    public LoginState(IAuthService authService)
    {
        _authService = authService;
    }

    public async Task<IState> Render()
    {
        _table = new Table().BorderColor(Color.Grey).Border(TableBorder.Ascii2).SafeBorder().Centered();
        _table.AddColumn("Label");
        _table.AddColumn("Input");

        RefreshTable();

        ICommandHandler command;
        do
        {
            command = await HandleInputAsync();
        }
        while (command == null);

        return await command.Execute();
    }

    private void RefreshTable()
    {
        AnsiConsole.Clear();
        _table.Rows.Clear();

        // Email
        _table.AddRow(
            new Text(_options[0], _selectedIndex == 0 ? new Style(foreground: Color.Yellow) : null),
            new Text(_email).LeftJustified()
        );

        // Password
        _table.AddRow(
            new Text(_options[1], _selectedIndex == 1 ? new Style(foreground: Color.Yellow) : null),
            new Text(_password).LeftJustified()
        );

        // Login and Signup
        _table.AddRow(
            new Text(_options[2], _selectedIndex == 2 ? new Style(foreground: Color.Yellow) : null),
            new Text(_options[3], _selectedIndex == 3 ? new Style(foreground: Color.Yellow) : null));

        AnsiConsole.Write(new Align(_table, HorizontalAlignment.Center, VerticalAlignment.Middle));
    }

    private Task<ICommandHandler> HandleInputAsync()
    {
        var key = Console.ReadKey(true);

        return key switch
        {
            { Key: ConsoleKey.UpArrow } or { Key: ConsoleKey.Tab, Modifiers: ConsoleModifiers.Shift } => HandlePrevious(),
            { Key: ConsoleKey.DownArrow } or { Key: ConsoleKey.Tab, Modifiers: ConsoleModifiers.None } => HandleNext(),
            { Key: ConsoleKey.Enter } => HandleAction(),
            _ => HandleTextPress()
        };

        Task<ICommandHandler> HandlePrevious()
        {
            _selectedIndex = (_selectedIndex - 1 + _options.Length) % _options.Length;
            RefreshTable();
            return Task.FromResult<ICommandHandler>(null);
        }

        Task<ICommandHandler> HandleNext()
        {
            _selectedIndex = (_selectedIndex + 1) % _options.Length;
            RefreshTable();
            return Task.FromResult<ICommandHandler>(null);
        }

        Task<ICommandHandler> HandleAction()
        {
            if (_selectedIndex == 2) // Go to Signup
                return Task.FromResult<ICommandHandler>(new GoToSignupCommand(_authService));
            if (_selectedIndex == 3) // Login
                return Task.FromResult<ICommandHandler>(new LoginCommandHandler(_authService, _email, _password));

            return Task.FromResult<ICommandHandler>(null);
        }

        Task<ICommandHandler> HandleTextPress()
        {
            if (_selectedIndex < 2) // Email or Password field
            {
                if (key.Key == ConsoleKey.Backspace)
                {
                    if (_selectedIndex == 0 && _email.Length > 0)
                        _email = _email[..^1];
                    else if (_selectedIndex == 1 && _password.Length > 0)
                        _password = _password[..^1];
                }
                else if (!char.IsControl(key.KeyChar))
                {
                    if (_selectedIndex == 0)
                        _email += key.KeyChar;
                    else
                        _password += key.KeyChar;
                }
                RefreshTable();
            }

            return Task.FromResult<ICommandHandler>(null);
        }
    }
}
