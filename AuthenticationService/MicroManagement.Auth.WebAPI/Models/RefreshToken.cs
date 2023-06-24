namespace MicroManagement.Auth.WebAPI.Models
{
    public class RefreshToken
    {
        public string Token { get; set; }
        public DateTime Expires { get; set; } // always Expressed in UTC
        public string UserId { get; set; }
    }
}
