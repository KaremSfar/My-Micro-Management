namespace MicroManagement.Auth.WebAPI.DTOs
{
    /// <summary>
    /// Wrapper of the Access Token returned by the service authentication endpoints
    /// </summary>
    public class JwtAccessTokenDTO
    {
        /// <summary>
        /// The actual JWT Token, has a 5 mins expiration time window
        /// </summary>
        public string AccessToken { get; set; }


        /// <summary>
        /// Initializes an instance of the <see cref="JwtAccessTokenDTO"/> class
        /// </summary>
        /// <param name="accessToken"></param>
        public JwtAccessTokenDTO(string accessToken)
        {
            AccessToken = accessToken;
        }
    }
}
