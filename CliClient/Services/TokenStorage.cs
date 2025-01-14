public class TokenStorage
{
    private string _token = "";

    public Task SaveToken(string token)
    {
        _token = token;
        return Task.CompletedTask;
    }

    public Task<string> GetToken()
    {
        return Task.FromResult(_token);
    }

    public Task ClearToken()
    {
        _token = "";
        return Task.CompletedTask;
    }
}