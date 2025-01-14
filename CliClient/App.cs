using CliClient.States;

public class App
{
    private readonly StateProvider _stateProvider;

    public App(StateProvider stateProvider)
    {
        _stateProvider = stateProvider;
    }

    public async Task RunAsync()
    {
        var state = _stateProvider.GetState("login");

        await state.Render();

        await Task.CompletedTask;
    }
}