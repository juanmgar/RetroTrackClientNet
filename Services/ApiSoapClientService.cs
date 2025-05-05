using RetroTrackSoapClient;

public class ApiSoapClientService
{
    private readonly UserManagementWSClient _soapClient;

    public ApiSoapClientService()
    {
        _soapClient = new UserManagementWSClient();
    }

    public async Task<string> RegisterUserAsync(string username, string password)
    {
        var response = await _soapClient.registerUserAsync(username, password);
        return response.Body.@return;
    }

    public async Task<string> LoginAsync(string username, string password)
    {
        var response = await _soapClient.loginAsync(username, password);
        return response.Body.@return;
    }
}
