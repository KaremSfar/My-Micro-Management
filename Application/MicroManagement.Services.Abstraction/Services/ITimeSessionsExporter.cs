using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MicroManagement.Services.Abstraction.Services
{
    public interface ITimeSessionsExporter
    {
        Task<string> ExportTimeSession();
    }
}
