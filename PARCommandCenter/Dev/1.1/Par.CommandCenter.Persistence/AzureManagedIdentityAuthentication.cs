using Microsoft.Azure.Services.AppAuthentication;
using Microsoft.Azure.Storage.Auth;
using System;
using System.Threading;
using System.Threading.Tasks;


namespace Par.CommandCenter.Persistence
{
    /// <summary>
    /// Class AzureManagedIdentityAuthentication.
    /// </summary>
    public class AzureManagedIdentityAuthentication
    {
        private string _resource = null;
        /// <summary>
        /// Initializes a new instance of the <see cref="AzureManagedIdentityAuthentication"/> class.
        /// </summary>
        /// <param name="resource">The resource.</param>
        public AzureManagedIdentityAuthentication(string resource)
        {
            _resource = resource;
        }
        /// <summary>
        /// Gets the access token.
        /// </summary>
        /// <returns>TokenCredential.</returns>
        public TokenCredential GetAccessToken()
        {
            // Get the initial access token and the interval at which to refresh it.
            AzureServiceTokenProvider azureServiceTokenProvider = new AzureServiceTokenProvider();

            var tokenAndFrequency = TokenRenewerAsync(azureServiceTokenProvider, CancellationToken.None).GetAwaiter().GetResult();

            // Create credentials using the initial token, and connect the callback function 
            // to renew the token just before it expires
            TokenCredential tokenCredential = new TokenCredential(tokenAndFrequency.Token,
                                                                    TokenRenewerAsync,
                                                                    azureServiceTokenProvider,
                                                                    tokenAndFrequency.Frequency.Value);
            return tokenCredential;
        }
        /// <summary>
        /// Renew the token
        /// </summary>
        /// <param name="state">The state.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>System.Threading.Tasks.Task&lt;Microsoft.Azure.Storage.Auth.NewTokenAndFrequency&gt;.</returns>
        private async Task<NewTokenAndFrequency> TokenRenewerAsync(Object state, CancellationToken cancellationToken)
        {
            // Use the same token provider to request a new token.
            var authResult = await ((AzureServiceTokenProvider)state).GetAuthenticationResultAsync(_resource);

            // Renew the token 5 minutes before it expires.
            var next = (authResult.ExpiresOn - DateTimeOffset.UtcNow) - TimeSpan.FromMinutes(5);
            if (next.Ticks < 0)
            {
                next = default(TimeSpan);
            }

            // Return the new token and the next refresh time.
            return new NewTokenAndFrequency(authResult.AccessToken, next);
        }
    }
}