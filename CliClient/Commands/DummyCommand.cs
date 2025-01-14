using CliClient.States;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CliClient.Commands
{
    internal class DummyCommand : ICommand
    {
        public IState Execute()
        {
            return new LoginState();
        }
    }
}
