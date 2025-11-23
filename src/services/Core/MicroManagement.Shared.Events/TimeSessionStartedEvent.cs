using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MicroManagement.Shared.Events
{
    public record TimeSessionStartedEvent
    {
        public Guid UserId { get; set; }
        public Guid ProjectId { get; set; }
    }
}
