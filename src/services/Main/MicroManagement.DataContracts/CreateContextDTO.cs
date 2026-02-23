using System;
using System.ComponentModel.DataAnnotations;

namespace MicroManagement.Services.Abstraction.DTOs
{
    /// <summary>
    /// The DataContract used to create a new Context
    /// </summary>
    public record CreateContextDTO
    {
        /// <summary>
        /// The Name of the context
        /// </summary>
        [Required]
        public string? Name { get; set; }

        /// <summary>
        /// The Icon representing the context
        /// </summary>
        [Required]
        public string? Icon { get; set; }
    }
}
