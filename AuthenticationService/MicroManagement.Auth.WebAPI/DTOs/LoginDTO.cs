using System.ComponentModel.DataAnnotations;

namespace MicroManagement.Auth.WebAPI.DTOs
{
    public class LoginDTO
    {
        [EmailAddress]
        [Required]
        public string Email { get; set; }

        [Required]
        public string Password { get; set; }
    }
}
