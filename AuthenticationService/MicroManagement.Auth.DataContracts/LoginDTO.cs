using System.ComponentModel.DataAnnotations;

namespace MicroManagement.Auth.WebAPI.DTOs
{
    /// <summary>
    /// DataContract for a user to login to the application
    /// </summary>
    public record LoginDTO
    {
        /// <summary>
        /// Email of the user
        /// </summary>
        [EmailAddress]
        [Required]
        public string? Email { get; set; }

        /// <summary>
        /// Password of the user
        /// </summary>
        [Required]
        public string? Password { get; set; }
    }
}
