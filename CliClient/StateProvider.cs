using CliClient.States;

public class StateProvider
{
    private readonly IServiceProvider _serviceProvider;

    public StateProvider(IServiceProvider serviceProvider)
    {
        this._serviceProvider = serviceProvider;
    }

    public IState GetState(string stateName)
    {
        return stateName switch
        {
            "login" => _serviceProvider.GetService(typeof(LoginState)) as IState,
            _ => null
        };
    }
}