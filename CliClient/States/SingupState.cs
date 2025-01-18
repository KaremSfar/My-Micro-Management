
using CliClient.Commands;
using Spectre.Console;

namespace CliClient.States
{
    internal class SingupState : IState
    {
        private IAuthService _authService;

        private string _email = "";
        private string _password = "";
        private string _firstName = "";
        private string _lastName = "";

        private int _selectedIndex = 0;
        private readonly string[] _options = { "FirstName", "LastName", "Email", "Password", "Go to Login", "Signup" };
        private Table _table;

        public SingupState(IAuthService authService)
        {
            this._authService = authService;
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
                new Text(_firstName).LeftJustified()
            );

            // Email
            _table.AddRow(
                new Text(_options[1], _selectedIndex == 1 ? new Style(foreground: Color.Yellow) : null),
                new Text(_lastName).LeftJustified()
            );

            // Email
            _table.AddRow(
                new Text(_options[2], _selectedIndex == 2 ? new Style(foreground: Color.Yellow) : null),
                new Text(_email).LeftJustified()
            );

            // Password
            _table.AddRow(
                new Text(_options[3], _selectedIndex == 3 ? new Style(foreground: Color.Yellow) : null),
                new Text(_password).LeftJustified()
            );

            // Login and Signup
            _table.AddRow(
                new Text(_options[4], _selectedIndex == 4 ? new Style(foreground: Color.Yellow) : null),
                new Text(_options[5], _selectedIndex == 5 ? new Style(foreground: Color.Yellow) : null));

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
                if (_selectedIndex == 4) // Go to Login
                    return Task.FromResult<ICommandHandler>(new GoToLoginCommand(_authService));
                if (_selectedIndex == 5) // Singup
                    return Task.FromResult<ICommandHandler>(new SignupCommand(_authService, _email, _password, _firstName, _lastName));

                return Task.FromResult<ICommandHandler>(null);
            }

            Task<ICommandHandler> HandleTextPress()
            {
                if (_selectedIndex < 4) // Email or Password field
                {
                    if (key.Key == ConsoleKey.Backspace)
                    {
                        if (_selectedIndex == 0 && _firstName.Length > 0)
                            _firstName = _firstName[..^1];
                        else if (_selectedIndex == 1 && _lastName.Length > 0)
                            _lastName = _lastName[..^1];
                        else if (_selectedIndex == 2 && _email.Length > 0)
                            _email = _email[..^1];
                        else if (_selectedIndex == 3 && _password.Length > 0)
                            _password = _password[..^1];
                    }
                    else if (!char.IsControl(key.KeyChar))
                    {
                        if (_selectedIndex == 0)
                            _firstName += key.KeyChar;
                        else if (_selectedIndex == 1)
                            _lastName += key.KeyChar;
                        else if (_selectedIndex == 2)
                            _email += key.KeyChar;
                        else if (_selectedIndex == 3)
                            _password += key.KeyChar;
                    }
                    RefreshTable();
                }

                return Task.FromResult<ICommandHandler>(null);
            }
        }
    }
}