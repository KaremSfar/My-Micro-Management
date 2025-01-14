using CliClient.Commands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CliClient.States
{
    /// <summary>
    /// A representation of a "View" in the app
    /// </summary>
    public interface IState
    {
        /// <summary>
        /// Renders the current state's UI
        /// </summary>
        Task<IState> Render();
    }
}
