using System.ComponentModel.DataAnnotations;

namespace MicroManagement.Auth.WebAPI.DTOs
{
    public class RegisterDTO
    {

        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        public string FirstName { get; set; }
        [Required]
        public string LastName { get; set; }

        [Required]
        public string Password { get; set; }
    }
}
