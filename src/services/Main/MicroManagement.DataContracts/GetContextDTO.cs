using System;

namespace MicroManagement.Services.Abstraction.DTOs
{
    /// <summary>
    /// The DataContract used to return Context data
    /// </summary>
    public record GetContextDTO
    {
        /// <summary>
        /// The Id of the context
        /// </summary>
        public Guid Id { get; set; }

        /// <summary>
        /// The Name of the context
        /// </summary>
        public string? Name { get; set; }

        /// <summary>
        /// The Icon representing the context
        /// </summary>
        public string? Icon { get; set; }
    }
}
