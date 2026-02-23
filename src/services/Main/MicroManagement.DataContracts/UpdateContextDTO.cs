using System;
using System.ComponentModel.DataAnnotations;

namespace MicroManagement.Services.Abstraction.DTOs
{
    /// <summary>
    /// The DataContract used to update an existing Context
    /// </summary>
    public record UpdateContextDTO
    {
        /// <summary>
        /// The updated Name of the context
        /// </summary>
        [Required]
        public string? Name { get; set; }

        /// <summary>
        /// The updated Icon representing the context
        /// </summary>
        [Required]
        public string? Icon { get; set; }
    }
}
