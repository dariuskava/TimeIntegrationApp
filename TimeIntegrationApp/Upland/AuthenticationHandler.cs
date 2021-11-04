using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Http;
using System.Net.Http.Headers;

namespace TimeIntegrationApp.Upland
{
    class AuthenticationHandler : IAuthenticationHandler
    {
        private readonly string Secret;
        private readonly log4net.ILog Log;
        private AccessToken token;
        private string userName;
        
        public AuthenticationHandler(string secret, log4net.ILog log)
        {
            Secret = secret;
            Log = log;
        }
        public void AddAuthenticationHeaders(System.Net.Http.Headers.HttpRequestHeaders headers)
        {
            headers.Authorization = new AuthenticationHeaderValue(GetToken().Token_type, GetToken().Access_token);
        }

        public AccessToken GetToken()
        {
            if (token == null)
            {
                token = RetrieveToken();
            }
            return token;
        }
        public int Uid { get; set; }
        public void SetUser(string userName, int uid)
        {
            this.userName = userName;
            this.Uid = uid;
        }
        
        private AccessToken RetrieveToken()
        {
            using (var client = new HttpClient())
            {
                StringContent content;
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Add("OrgName", Config.UplandOrgName);
                content = new StringContent("grant_type=password&username=" + userName + "&password=" + Secret);
                var response = client.PostAsync(ApiUrl.Token, content).Result;
                response.EnsureSuccessStatusCode();
                var token = response.Content.ReadAsAsync<AccessToken>().Result;
                return token;
            }
        }
    }
}
