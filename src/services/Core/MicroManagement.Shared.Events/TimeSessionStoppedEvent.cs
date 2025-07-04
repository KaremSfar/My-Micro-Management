using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MicroManagement.Shared.Events
{
    public class TimeSessionStoppedEvent
    {
        public Guid UserId { get; set; }
    }
}
