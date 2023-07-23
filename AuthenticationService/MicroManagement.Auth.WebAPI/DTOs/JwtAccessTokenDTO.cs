namespace MicroManagement.Auth.WebAPI.DTOs
{
    public class JwtAccessTokenDTO
    {
        public string AccessToken { get; set; }

        public JwtAccessTokenDTO(string accessToken)
        {
            AccessToken = accessToken;
        }
    }
}
