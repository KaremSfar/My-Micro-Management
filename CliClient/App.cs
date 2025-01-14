using CliClient.States;

public class App
{
    public App()
    {
    }

    public async Task RunAsync()
    {
        var state = new LoginState();

        await state.Render();

        await Task.CompletedTask;
    }
}