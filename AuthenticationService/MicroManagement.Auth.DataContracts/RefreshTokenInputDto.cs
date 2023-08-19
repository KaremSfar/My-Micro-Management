using System.ComponentModel.DataAnnotations;

namespace MicroManagement.Auth.WebAPI.DTOs
{
    public class RefreshTokenInputDto
    {
        [Required] public string RefreshToken { get; set; }
    }
}
