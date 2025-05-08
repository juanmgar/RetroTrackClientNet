using RetroTrackSoapClient;

public class ApiSoapClientService
{
    private readonly UserManagementWSClient _soapClient;

    public ApiSoapClientService()
    {
        _soapClient = new UserManagementWSClient();

        //Modificación para poder usar certificados sin firmar
        _soapClient.ClientCredentials.ServiceCertificate.SslCertificateAuthentication =
        new System.ServiceModel.Security.X509ServiceCertificateAuthentication()
        {
            CertificateValidationMode = System.ServiceModel.Security.X509CertificateValidationMode.None,
            RevocationMode = System.Security.Cryptography.X509Certificates.X509RevocationMode.NoCheck
        };
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
