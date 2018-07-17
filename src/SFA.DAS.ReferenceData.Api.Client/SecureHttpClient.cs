using System;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.IdentityModel.Clients.ActiveDirectory;

namespace SFA.DAS.ReferenceData.Api.Client
{
    internal class SecureHttpClient
    {
        private readonly IReferenceDataApiConfiguration _configuration;

        public SecureHttpClient(IReferenceDataApiConfiguration configuration)
        {
            _configuration = configuration;
        }
        protected SecureHttpClient()
        {
            // So we can mock for testing
        }

        private async Task<AuthenticationResult> GetAuthenticationResult(string clientId, string appKey, string resourceId, string tenant)
        {
            var authority = $"https://login.microsoftonline.com/{tenant}";
            var clientCredential = new ClientCredential(clientId, appKey);
            var context = new AuthenticationContext(authority, true);
            var result = await context.AcquireTokenAsync(resourceId, clientCredential);
            return result;
        }

        public virtual Task<string> GetAsync(string url, bool exceptionOnNotFound = true)
        {
            return GetAsync(url, response =>
            {
                if (!exceptionOnNotFound && response.StatusCode == HttpStatusCode.NotFound)
                {
                    return false;
                }

                return true;
            });
        }

        /// <summary>
        ///     Returns the string content of the target URL. 
        /// </summary>
        /// <param name="url"></param>
        /// <param name="responseChecker">
        ///     A delegate that will be invoked after the response is received but before the content has been requested.
        ///     The delegate will have the opportunity to check the status code and decide whether to continue to fetch the
        ///     actual content (return false to skip the content checking).
        /// </param>
        public virtual async Task<string> GetAsync(string url, Func<HttpResponseMessage, bool> responseChecker)
        {
            var authenticationResult = await GetAuthenticationResult(_configuration.ClientId,
                _configuration.ClientSecret, _configuration.IdentifierUri, _configuration.Tenant);

            using (var client = new HttpClient())
            {
                client.DefaultRequestHeaders.Authorization =
                    new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", authenticationResult.AccessToken);

                var response = await client.GetAsync(url);

                if (responseChecker != null)
                {
                    if (!responseChecker(response))
                    {
                        return null;
                    }
                }

                response.EnsureSuccessStatusCode();

                return await response.Content.ReadAsStringAsync();
            }
        }
    }
}
