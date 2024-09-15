using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MicroManagement.Services.Abstraction.DTOs
{
    /// <summary>
    /// The DataContract used to add Projects to a user's collection of projects, and the same DataContract returned when fetching the collection
    /// </summary>
    public record ProjectSessionDTO
    {
        /// <summary>
        /// The Id of the Project
        /// </summary>
        public Guid Id { get; set; }

        /// <summary>
        /// The Name of the project
        /// </summary>
        public string? Name { get; set; }

        /// <summary>
        /// The Color of the project, this is used to personalize the experience on the application
        /// </summary>
        public string? Color { get; set; }

        /// <summary>
        /// Aggregated State over TimeSessions of this project, informs client if a current time session with this project is running
        /// </summary>
        public bool IsRunning { get; set; }

        /// <summary>
        /// The Time Spent during this Active Sessions
        /// </summary>
        public double TimeSpentCurrentSession { get; set; }

        /// <summary>
        /// The Total Time spent on this Project, ever
        /// </summary>
        public double TimeSpentTotal { get; set; }
    }
}
