using MicroManagement.Services.Abstraction.DTOs;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace My_Micro_Management.Features.Timer.ViewModels
{
    public class TimerViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        private ProjectDTO selectedProject;
        public ProjectDTO SelectedProject
        {
            get { return selectedProject; }
            set { selectedProject = value; OnPropertyChanged(); }
        }

        private TimeSpan timeElapsed = new TimeSpan();
        public TimeSpan TimeElapsed
        {
            get { return timeElapsed; }
            set { timeElapsed = value; OnPropertyChanged(); }
        }

        public TimerViewModel()
        {
        }

        private void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
