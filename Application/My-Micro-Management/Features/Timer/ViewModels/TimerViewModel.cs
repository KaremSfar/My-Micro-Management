using MicroManagement.Services.Abstraction.DTOs;
using MicroManagement.Services.Abstraction.Services;
using MicroManagement.Services.Mock;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace My_Micro_Management.Features.Timer.ViewModels
{
    public class TimerViewModel : INotifyPropertyChanged, IDisposable
    {
        public event PropertyChangedEventHandler PropertyChanged;
        private ITimeSessionsService _timeSessionsService = new MockTimeSessionsService();

        private ProjectDTO selectedProject;
        public ProjectDTO SelectedProject
        {
            get { return selectedProject; }
            set
            {
                // Start the timer on the First Project Selection
                if (selectedProject == null)
                    StartTimer();
                else
                    OnProjectChanged();

                selectedProject = value;
                OnPropertyChanged();
            }
        }

        private TimeSpan _timeElapsed = new TimeSpan();
        private TimeSpan TimeElapsedSpan
        {
            get { return _timeElapsed; }
            set
            {
                _timeElapsed = value;
                OnPropertyChanged();
                OnPropertyChanged(nameof(TimeElapsedStr));
            }
        }

        public string TimeElapsedStr
        {
            get
            {
                return $"{TimeElapsedSpan.Minutes:00}:{TimeElapsedSpan.Seconds:00}";
            }
            set
            {
                throw new NotImplementedException();
            }
        }

        private CancellationTokenSource cancelTokenSource = new CancellationTokenSource();

        public TimerViewModel()
        {
            TimeElapsedSpan = new TimeSpan();
        }

        private void StartTimer()
        {
            Task.Factory.StartNew(async () =>
            {
                while (true && !cancelTokenSource.IsCancellationRequested)
                {
                    await Task.Delay(1000);
                    TimeElapsedSpan = TimeElapsedSpan.Add(TimeSpan.FromSeconds(1));
                }
            }, cancelTokenSource.Token);
        }
        private void ResetElapsed()
        {
            TimeElapsedSpan = TimeSpan.Zero;
        }

        private async Task OnProjectChanged()
        {
            var timeSession = new TimeSessionDTO()
            {
                StartTime = DateTime.Now - _timeElapsed,
                EndDate = DateTime.Now
            };

            await _timeSessionsService.AddTimeSession(timeSession);

            ResetElapsed();
        }

        private void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public void Dispose()
        {
            cancelTokenSource.Dispose();
        }
    }
}
