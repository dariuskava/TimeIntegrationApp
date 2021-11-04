

namespace TimeIntegrationApp
{
    interface IAuthenticationHandler
    {
        void AddAuthenticationHeaders(System.Net.Http.Headers.HttpRequestHeaders headers);

    }
}
