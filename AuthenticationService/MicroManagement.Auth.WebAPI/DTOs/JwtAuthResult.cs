namespace MicroManagement.Auth.WebAPI.DTOs
{
    public class JwtAuthResult
    {
        public string AccessToken { get; set; }
        public string RefreshToken { get; set; }
    }
}
