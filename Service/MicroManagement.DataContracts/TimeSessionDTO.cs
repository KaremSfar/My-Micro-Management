using System.ComponentModel.DataAnnotations;

namespace MicroManagement.Services.Abstraction.DTOs
{

    /// <summary>
    /// DataContract used to Add TimeSessions into a user's collection. Also same DTO used when fetching the collection.
    /// </summary>
    public record TimeSessionDTO
    {
        /// <summary>
        /// Start time of the session
        /// </summary>
        [Required]
        public DateTime StartTime { get; set; }

        /// <summary>
        /// End Time of the session 
        /// </summary>
        [Required]
        public DateTime EndDate { get; set; } // TODO-KAREM: rename this into EndTime :(

        /// <summary>
        /// The projects that were in action during the time session
        /// </summary>
        [Required, MinLength(1)]
        public IList<Guid> ProjectIds { get; set; } // TODO: Turn into a single Project for each session
    }
}