using MicroManagement.Services.Abstraction;
using MicroManagement.Services.Abstraction.DTOs;
using MicroManagement.Services.Mock;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace My_Micro_Management.Features.ProjectsPanel
{
    public class ProjectsListViewModel
    {
        private IProjectsService _projectsService = ProjectsServiceFactory.Instance;
        public ObservableCollection<ProjectDTO> ProjectsDTOs { get; set; }

        public ProjectsListViewModel()
        {
            ProjectsDTOs = new ObservableCollection<ProjectDTO>();
            FetchProjects();
        }

        public async Task FetchProjects()
        {
            ProjectsDTOs.Clear();

            var projects = await _projectsService.GetAll();
            foreach (var project in projects)
            {
                ProjectsDTOs.Add(project);
            }
        }
    }
}
