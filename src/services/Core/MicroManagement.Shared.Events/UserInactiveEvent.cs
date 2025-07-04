using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MicroManagement.Shared.Events
{
    public record UserInactiveEvent
    {
        public Guid UserId { get; init; }
        public DateTime InactiveSince { get; init; }
    }
}
