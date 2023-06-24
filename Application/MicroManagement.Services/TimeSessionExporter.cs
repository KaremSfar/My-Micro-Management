using MicroManagement.Services.Abstraction;
using System.Text;
using System.Linq;
using MicroManagement.Application.Services.Abstraction;

namespace MicroManagement.Application.Services
{
    public class TimeSessionExporter : ITimeSessionsExporter
    {
        private ITimeSessionsService _timeService;
        private IProjectsService _projectsService;

        public TimeSessionExporter(ITimeSessionsService timeService, IProjectsService projectsService)
        {
            _timeService = timeService;
            _projectsService = projectsService;
        }

        public async Task<string> ExportTimeSession()
        {
            var allSession = await _timeService.GetAll();
            var allProjects = (await _projectsService.GetAll())
                .ToDictionary(p => p.Id, p => p.Name);

            var sessions = from timeSession in allSession
                           where timeSession.StartTime > DateTime.Today
                           orderby timeSession.StartTime
                           select new
                           {
                               SessionProject = allProjects[timeSession.ProjectIds.First()],
                               SessionStartTime = timeSession.StartTime,
                               SessionEndTime = timeSession.EndDate,
                               TimeSpent = timeSession.EndDate - timeSession.StartTime
                           };

            var stringBuilder = new StringBuilder("Start;End;Project;Elapsed" + Environment.NewLine);

            foreach (var session in sessions)
            {
                var line = $"{session.SessionStartTime};{session.SessionEndTime};{session.SessionProject};{session.TimeSpent}";
                stringBuilder.AppendLine(line);
            }

            return stringBuilder.ToString();
        }
    }
}