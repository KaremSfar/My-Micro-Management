using System.ComponentModel.DataAnnotations;

namespace MicroManagement.Auth.WebAPI.DTOs
{
    /// <summary>
    /// The DataContract for registering a new user to the application
    /// </summary>
    public record RegisterDTO
    {

        /// <summary>
        /// The user's email, must be unique and never used before
        /// </summary>
        [Required]
        [EmailAddress]
        public string Email { get; set; }

        /// <summary>
        /// The user's first name
        /// </summary>
        [Required]
        public string FirstName { get; set; }

        /// <summary>
        /// The user's last name
        /// </summary>
        [Required]
        public string LastName { get; set; }

        /// <summary>
        /// The user's password to login to the application
        /// </summary>
        [Required]
        public string Password { get; set; }
    }
}
