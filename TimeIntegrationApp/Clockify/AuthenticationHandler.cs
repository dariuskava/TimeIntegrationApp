using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TimeIntegrationApp.Clockify
{
    class AuthenticationHandler : IAuthenticationHandler
    {
        private readonly string Secret;
        private readonly log4net.ILog Log;
        public AuthenticationHandler(string secret, log4net.ILog log)
        {
            Secret = secret;
            Log = log;
        }
        public void AddAuthenticationHeaders(System.Net.Http.Headers.HttpRequestHeaders headers)
        {
            headers.Add("X-Api-Key", Secret);               
        }
 
    }
}
