using JetBrains.Annotations;
using Newtonsoft.Json;
using Par.CommandCenter.Application.Interfaces;
using Par.CommandCenter.AzureMapAPI.ConfigurationOptions;
using Par.CommandCenter.Domain.Model;
using System;
using System.Linq;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace Par.CommandCenter.AzureMapAPI
{
    public class AzureMapAPIWebClient : IAzureMapAPIWebClient
    {
        [NotNull]
        private readonly IHttpClientFactory _clientFactory;

        [NotNull]
        private readonly AzureMapAPIConfigurationOptions _azureMapAPISetting;

        public AzureMapAPIWebClient([NotNull] AzureMapAPIConfigurationOptions azureMapAPISetting, [NotNull] IHttpClientFactory clientFactory)
        {
            _azureMapAPISetting = azureMapAPISetting ?? throw new ArgumentNullException(nameof(azureMapAPISetting));
            _clientFactory = clientFactory;
        }

        public async Task<GeoCoordinate> GetAddressCoordinates(Address address, CancellationToken cancellationToken)
        {
            if (string.IsNullOrWhiteSpace(address.AddressLine1) && string.IsNullOrWhiteSpace(address.City))
            {
                throw new ArgumentNullException(nameof(address));
            }

            try
            {
                var client = _clientFactory.CreateClient("GetAddressCoordinates");
                client.Timeout = TimeSpan.FromMinutes(2);

                var url = $"{_azureMapAPISetting.AzureMapSearchApiURL}&subscription-key={_azureMapAPISetting.AzureMapSubscriptionKey}&limit=1&query={$"{address.AddressLine1} {address.City} {address.PostalCode ?? string.Empty}"}";

                HttpResponseMessage response = await client.GetAsync(url, cancellationToken).ConfigureAwait(false);

                // Request was submitted successfully
                if (response.IsSuccessStatusCode)
                {
                    AzureMapSearchResponse responseContent = JsonConvert.DeserializeObject<AzureMapSearchResponse>(await response.Content.ReadAsStringAsync());

                    if (responseContent != null && responseContent.Results.Count() == 1)
                    {
                        return new GeoCoordinate(responseContent.Results.FirstOrDefault().Position.Lat, responseContent.Results.FirstOrDefault().Position.Lon, address);
                    }
                    else
                    {
                        //throw new Exception(message: "Something Went Wrong! Error Occured");
                        return null;
                    }
                }
                else
                {
                    throw new Exception(message: "Something Went Wrong! Error Occured");
                }
            }
            catch (AggregateException)
            {
                throw;
            }
            catch (OperationCanceledException)
            {
                throw;
            }
            catch (Exception)
            {
                Console.WriteLine($"Something Went Wrong! Error Occured for Address: {address.AddressLine1} , City: {address.City} , Postal Code: {address.PostalCode}");
                throw;
            }
        }
    }
}