using MicroManagement.Services.Abstraction;
using System.Text;
using System.Linq;
using MicroManagement.Application.Services.Abstraction;

namespace MicroManagement.Application.Services
{
    public class TimeSessionExporter : ITimeSessionsExporter
    {
        public TimeSessionExporter()
        {
        }

        public async Task<string> ExportTimeSession()
        {
            return string.Empty;
        }
    }
}