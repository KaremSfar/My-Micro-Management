using CliClient.States;

namespace CliClient.Commands
{
    public interface ICommandHandler
    {
        Task<IState> Execute();
    }
}
