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

        do
        {
            state = await state.Render();
        } while (state != null);

        await Task.CompletedTask;
    }
}