using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MicroManagement.Services.Abstraction.DTOs
{
    /// <summary>
    /// The DataContract used to add Projects to a user's collection of projects
    /// </summary>
    public record GetProjectDTO
    {
        /// <summary>
        /// The Id of the Project
        /// </summary>
        public Guid Id { get; set; }
        /// <summary>
        /// The Name of the project, is NOT a unique identifier.
        /// </summary>
        public string? Name { get; set; }

        /// <summary>
        /// The Color of the project, this is used to personalize the experience on the application
        /// </summary>
        public string? Color { get; set; }
    }
}
