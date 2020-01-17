using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Clients.ActiveDirectory;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace CRMAPI.CRMFunctions
{
    public class Authentication
    {
        public AuthenticationResult Result { get; private set; }

        public string CRMOrgURL { get; private set; }
        private IConfigurationRoot Configuration { get; set; }

        public Authentication()
        {
            if (Result == null)
            {
                Configuration = new ConfigurationBuilder()
                        .SetBasePath(Directory.GetCurrentDirectory())
                        .AddJsonFile("appsettings.json")
                        .Build();
                Result = GetToken().Result;
            }
        }
        private async Task<AuthenticationResult> GetToken()
        {
            try
            {                
                ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
                AuthenticationParameters ap = AuthenticationParameters.CreateFromUrlAsync(new Uri(Configuration["CRMConnection:ServiceUrl"])).Result;

                string authorityUrl = ap.Authority.Remove(ap.Authority.IndexOf("/oauth2/authorize"));

                AuthenticationContext authContext = new Microsoft.IdentityModel.Clients.ActiveDirectory.AuthenticationContext(authorityUrl, false);

                CRMOrgURL = ap.Resource;

                try
                {
                    return await GetAccessToken(authorityUrl, CRMOrgURL, Configuration["CRMConnection:ClientID"], Configuration["CRMConnection:ClientSecret"]);
                }
                catch (AdalException e)
                {
                    throw e;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private async Task<AuthenticationResult> GetAccessToken(string authority, string resource, string clientId, string clientSecret)
        {
            var clientCredential = new ClientCredential(clientId, clientSecret);
            AuthenticationContext context = new AuthenticationContext(authority, false);
            AuthenticationResult authenticationResult = await context.AcquireTokenAsync(resource, clientCredential);
            return authenticationResult;
        }
    }
}
