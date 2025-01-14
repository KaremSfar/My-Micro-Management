using CliClient.Commands;
using CliClient.States;
using Spectre.Console;
using System;

public class LoginState : IState
{
    private string _email = "";
    private string _password = "";
    private int _selectedIndex = 0;
    private readonly string[] _options = { "Email", "Password", "Go to Signup", "Login" };
    private Table _table;
    private LiveDisplayContext _context;

    public async Task<ICommand> Render()
    {
        _table = new Table().BorderColor(Color.Grey).Border(TableBorder.Square);
        _table.AddColumn("Label");
        _table.AddColumn("Input");

        RefreshTable();

        ICommand command;
        do
        {
            command = await HandleInputAsync();
        }
        while (command == null);

        return command;
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

        AnsiConsole.Write(_table);
    }

    public Task<ICommand> HandleInputAsync()
    {
        var key = Console.ReadKey(true);
        return key switch
        {
            { Key: ConsoleKey.UpArrow } or { Key: ConsoleKey.Tab, Modifiers: ConsoleModifiers.None } => HandlePrevious(),
            { Key: ConsoleKey.DownArrow } or { Key: ConsoleKey.Tab, Modifiers: ConsoleModifiers.Shift } => HandleNext(),
            { Key: ConsoleKey.Enter } => HandleAction(),
            _ => HandleTextPress()
        };

        Task<ICommand> HandlePrevious()
        {
            _selectedIndex = (_selectedIndex - 1 + _options.Length) % _options.Length;
            RefreshTable();
            return Task.FromResult<ICommand>(null);
        }

        Task<ICommand> HandleNext()
        {
            _selectedIndex = (_selectedIndex + 1) % _options.Length;
            RefreshTable();
            return Task.FromResult<ICommand>(null);
        }

        Task<ICommand> HandleAction()
        {
            if (_selectedIndex == 2) // Go to Signup
                return Task.FromResult<ICommand>(new DummyCommand());
            if (_selectedIndex == 3) // Login
                return Task.FromResult<ICommand>(new DummyCommand());

            return Task.FromResult<ICommand>(null);
        }

        Task<ICommand> HandleTextPress()
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

            return Task.FromResult<ICommand>(null);
        }
    }
}
