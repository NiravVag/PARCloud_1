using Azure.Core;
using Azure.Identity;
using Par.CommandCenter.Application.Interfaces;
using Par.CommandCenter.ProductsWebAPI.ConfigurationOptions;
using Products.Web.API.Client;
using System;
using System.Diagnostics.CodeAnalysis;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

using System.Linq;
using Microsoft.Identity.Client;
using Microsoft.Identity.Web;
using IProductsWebAPIClient = Products.Web.API.Client.IProductsWebAPIClient;

namespace Par.CommandCenter.ProductsWebAPI
{
    public class ProductsWebAPIClient : IProductsWebAPIClient
    {
        [NotNull]
        private readonly IHttpClientFactory _clientFactory;

        [NotNull]
        private readonly ProductsWebAPIConfigurationOptions _productsApiSetting;

        [NotNull]
        private readonly Products.Web.API.Client.ProductsWebAPIClient _client;

        [NotNull]
        private readonly ITokenAcquisition _tokenAcquisition;

        public ProductsWebAPIClient([NotNull] ProductsWebAPIConfigurationOptions productsApiSetting, [NotNull] IHttpClientFactory clientFactory, ITokenAcquisition tokenAcquisition)
        {
            _productsApiSetting = productsApiSetting ?? throw new ArgumentNullException(nameof(productsApiSetting));
            _clientFactory = clientFactory;
            var myClient = _clientFactory.CreateClient();


            ////TokenCredential tokenCredential = new VisualStudioCredential(new VisualStudioCredentialOptions() { AuthorityHost = new Uri("https://login.microsoftonline.com/parexcellencesystems.onmicrosoft.com/oauth2/v2.0/authorize") });

            ////TokenRequestContext requestContext = new TokenRequestContext(new string[] { "api://2775b3c0-7c92-44c6-8f0c-cc753e48f816/.default" });
            ////CancellationTokenSource cts = new CancellationTokenSource();
            ////var accessToken = tokenCredential.GetToken(requestContext, cts.Token).Token;


            ////DefaultAzureCredentialOptions defaultAzureCredentialOptions = new DefaultAzureCredentialOptions();
            ////defaultAzureCredentialOptions.ManagedIdentityClientId = "a907017d-d02e-4c76-ba40-87ace5273d50";
            ////defaultAzureCredentialOptions.AuthorityHost = new Uri(productsApiSetting.Authority);//new Uri("https://login.microsoftonline.com/parexcellencesystems.onmicrosoft.com/oauth2/v2.0/authorize");
            ////defaultAzureCredentialOptions.InteractiveBrowserTenantId = "37e40552-cb8a-4398-9aa2-69d6f71aebec";
            ////defaultAzureCredentialOptions.SharedTokenCacheTenantId = "37e40552-cb8a-4398-9aa2-69d6f71aebec";
            ////defaultAzureCredentialOptions.ExcludeEnvironmentCredential = true;

            ////DefaultAzureCredential tokenCredential = new DefaultAzureCredential(defaultAzureCredentialOptions);

            ////VisualStudioCredentialOptions visualStudioCredentialOptions = new VisualStudioCredentialOptions();
            ////visualStudioCredentialOptions.AuthorityHost = new Uri("https://login.microsoftonline.com/parexcellencesystems.onmicrosoft.com/oauth2/v2.0/authorize");
            ////visualStudioCredentialOptions.Cl

            ////VisualStudioCredential credential = new VisualStudioCredential()
            ////string accessToken = tokenCredential.GetToken(
            ////    new Azure.Core.TokenRequestContext(new[] { "api://2775b3c0-7c92-44c6-8f0c-cc753e48f816/.default" })).Token.ToString();

            ////string accessToken = tokenCredential.GetToken(
            ////    new Azure.Core.TokenRequestContext(new[] { "api://2775b3c0-7c92-44c6-8f0c-cc753e48f816/ReadWriteAccess" })).Token.ToString();


            ////var app = PublicClientApplicationBuilder.Create(_productsApiSetting.ClientId)
            ////   .WithAuthority(_productsApiSetting.Authority)
            ////   .WithRedirectUri(_productsApiSetting.RedirectUri)
            ////   .Build();

            ////var result = app.AcquireTokenSilentAsync(scopes, ).ExecuteAsync().Result;

            ////var accessToken = result.AccessToken;

            _tokenAcquisition = tokenAcquisition;

            var accessToken = _tokenAcquisition.GetAccessTokenForUserAsync(productsApiSetting.Scopes).Result;
            _client = new Products.Web.API.Client.ProductsWebAPIClient(productsApiSetting.BaseUrl, myClient);
            _client.SetBearerToken(accessToken);
        }        

        public async Task<GetAllProductsDtoPaginatedList> ProductsGETAsync(int? tenantId, bool? includeTenants, bool? includeItems, int? pageNumber, int? pageSize, CancellationToken cancellationToken)
        {          
            return await _client.ProductsGETAsync(tenantId, includeTenants, includeItems , pageNumber, pageSize, cancellationToken).ConfigureAwait(false);           
        }

        public async Task<GetAllProductsDtoPaginatedList> ProductsGETAsync(int? tenantId, bool? includeTenants, bool? includeItems, int? pageNumber, int? pageSize)
        {
            return await _client.ProductsGETAsync(tenantId, includeTenants, includeItems,pageNumber, pageSize).ConfigureAwait(false);
        }

        public async Task<GetAllProductsDtoPaginatedList> ProductsPOSTAsync(GetAllProductsWithPaginationQuery body)
        {
            return await _client.ProductsPOSTAsync(body).ConfigureAwait(false);
        }

        public async Task<GetAllProductsDtoPaginatedList> ProductsPOSTAsync(GetAllProductsWithPaginationQuery body, CancellationToken cancellationToken)
        {
            return await _client.ProductsPOSTAsync(body, cancellationToken).ConfigureAwait(false);
        }
    }
}



