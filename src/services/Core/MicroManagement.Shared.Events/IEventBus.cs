using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MicroManagement.Shared.Events
{
    public interface IEventBus<in TEvent>
    {
        Task PublishAsync(TEvent e, CancellationToken cancellationToken = default);
    }
}
