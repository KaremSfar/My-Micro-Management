using RestSharp;
using System.IdentityModel.Tokens.Jwt;


public interface IAuthService
{
    bool IsAuthenticated { get; }
    Task<string> GetTokenAsync();
    Task LoginAsync(string email, string password);
    Task SignupAsync(string email, string password, string firstName, string lastName);
    Task LogoutAsync();
}

public class AuthService : IAuthService
{
    private readonly RestClient _restClient;
    private string _jwtToken;
    private TokenStorage _tokenStorage;

    public bool IsAuthenticated => !string.IsNullOrEmpty(_jwtToken);

    public AuthService(string baseUrl, TokenStorage tokenStorage)
    {
        _restClient = new RestClient(baseUrl);
        _tokenStorage = tokenStorage;
    }

    public async Task LoginAsync(string email, string password)
    {
        var request = new RestRequest("Auth/login", Method.Post);
        request.AddJsonBody(new { email, password });

        var response = await _restClient.ExecuteAsync<TokenResponse>(request);

        if (response.IsSuccessful)
        {
            var tokens = response.Data;
            _jwtToken = tokens!.AccessToken;
            var refresh = response.Cookies["refreshToken"]!.Value;

            await _tokenStorage.SaveToken(refresh);
        }
    }

    public async Task<string> GetTokenAsync()
    {
        // Load the token if not already in memory
        if (_jwtToken == null)
        {
            _jwtToken = await _tokenStorage.GetToken();
        }

        // Check token expiration and refresh if necessary
        if (IsTokenExpired(_jwtToken))
        {
            await RefreshTokenAsync();
        }

        return _jwtToken;
    }

    private bool IsTokenExpired(string token)
    {
        if (string.IsNullOrEmpty(token))
            return true;

        var jwtToken = new JwtSecurityTokenHandler().ReadJwtToken(token);
        return jwtToken.ValidTo < DateTime.UtcNow;
    }

    private async Task RefreshTokenAsync()
    {
        // Implement your refresh logic here
        var request = new RestRequest("api/auth/refresh", Method.Post);
        request.AddJsonBody(new { token = _jwtToken });

        var response = await _restClient.ExecuteAsync<TokenResponse>(request);

        if (response.IsSuccessful)
        {
            var tokens = response.Data;
            _jwtToken = tokens!.AccessToken;

            var refresh = response.Cookies["refreshToken"];

            await _tokenStorage.SaveToken(_jwtToken);
        }
        else
        {
            throw new Exception("Token refresh failed");
        }
    }

    public async Task LogoutAsync()
    {
        _jwtToken = null;
        await _tokenStorage.ClearToken();
    }

    public async Task SignupAsync(string email, string password, string firstName, string lastName)
    {
        var request = new RestRequest("Auth/Register", Method.Post);
        request.AddJsonBody(new { email, password, firstName, lastName });

        var response = await _restClient.ExecuteAsync<TokenResponse>(request);

        if (response.IsSuccessful)
        {
            var tokens = response.Data;
            _jwtToken = tokens!.AccessToken;
            var refresh = response.Cookies["refreshToken"]!.Value;

            await _tokenStorage.SaveToken(refresh);
        }
    }
}

public class TokenResponse
{
    public string AccessToken { get; set; }
}