using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MicroManagement.Shared.Events
{
    public interface IEventBusListener<in TEvent>
    {
        Task OnEventAsync(TEvent e, CancellationToken cancellationToken = default);
    }
}
