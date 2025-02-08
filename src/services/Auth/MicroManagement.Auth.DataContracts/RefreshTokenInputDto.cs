using System.ComponentModel.DataAnnotations;

namespace MicroManagement.Auth.WebAPI.DTOs
{
    /// <summary>
    /// Wrapper for the Refresh Token, originally received from the service to refresh the access token
    /// </summary>
    public record RefreshTokenInputDto
    {
        /// <summary>
        /// The actual Refresh token, JWT with long expiration date
        /// </summary>
        [Required]
        public string? RefreshToken { get; set; }
    }
}
