using CliClient.States;

namespace CliClient.Commands
{
    public interface ICommand
    {
        IState Execute();
    }
}
