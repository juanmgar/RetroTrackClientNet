using RetroTrackSoapClient;
using System.ServiceModel;

public class ApiSoapClientService
{
    private readonly UserManagementWSClient _soapClient;

    public ApiSoapClientService(string endpointUrl)
    {
        var binding = new BasicHttpBinding(BasicHttpSecurityMode.Transport); // Usa HTTPS, si usas HTTP → usa None
        var endpoint = new EndpointAddress(endpointUrl);

        _soapClient = new UserManagementWSClient(binding, endpoint);

        // Permitir certificados autofirmados
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
