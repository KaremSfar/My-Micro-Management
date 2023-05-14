using MicroManagement.Services.Abstraction;
using MicroManagement.Services.Mock;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace My_Micro_Management.Features.ProjectsPanel
{
    internal static class ProjectsServiceFactory
    {
        static public IProjectsService Instance { get; } = new MockProjectsService();
    }
}
