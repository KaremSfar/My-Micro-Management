using CliClient.States;

namespace CliClient.Commands;

public class DummyCommand : ICommandHandler
{
    public Task<IState> Execute()
    {
        return Task.FromResult<IState>(null);
    }
}
