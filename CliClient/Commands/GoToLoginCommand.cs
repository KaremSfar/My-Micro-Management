using CliClient.States;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CliClient.Commands
{
    public class GoToLoginCommand : ICommandHandler
    {
        private readonly IAuthService authService;

        public GoToLoginCommand(IAuthService authService)
        {
            this.authService = authService;
        }

        public Task<IState> Execute()
        {
            return Task.FromResult<IState>(new LoginState(this.authService));
        }
    }
}
