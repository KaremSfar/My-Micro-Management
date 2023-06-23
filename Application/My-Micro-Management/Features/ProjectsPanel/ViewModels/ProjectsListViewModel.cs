using MicroManagement.Application.Services.Abstraction;
using MicroManagement.Services.Abstraction;
using MicroManagement.Services.Abstraction.DTOs;
using MicroManagement.Services.Mock;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace My_Micro_Management.Features.ProjectsPanel
{
    public class ProjectsListViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        private IProjectsService _projectsService = MauiProgram.GetService<IProjectsService>();
        public ObservableCollection<ProjectDTO> ProjectsDTOs { get; set; }

        private ProjectDTO selectedProject;
        public ProjectDTO SelectedProject
        {
            get { return selectedProject; }
            set { selectedProject = value; OnPropertyChanged(); }
        }

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

        private void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
