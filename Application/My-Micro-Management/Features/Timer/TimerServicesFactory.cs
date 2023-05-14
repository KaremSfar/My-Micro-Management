using MicroManagement.Services.Abstraction;
using MicroManagement.Services.Abstraction.Services;
using MicroManagement.Services.Mock;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace My_Micro_Management.Features.Timer
{
    internal static class TimerServicesFactory
    {
        static public ITimeSessionsService Instance { get; } = new MockTimeSessionsService();
    }
}
