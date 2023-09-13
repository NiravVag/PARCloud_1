using Newtonsoft.Json;
using Par.CommandCenter.Application.Interfaces;
using Par.CommandCenter.CloudDeviceManagement.ConfigurationOptions;
using Par.CommandCenter.Domain.Model;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace Par.CommandCenter.CloudDeviceManagement
{
    public class AzureFunctionsClient : IAzureFunctionsClient
    {
        private sealed class ResponseData
        {
            public decimal? Weight { get; set; }
            public int? Quantity { get; set; }
        }

        [NotNull]
        private readonly IHttpClientFactory _clientFactory;

        [NotNull]
        private readonly AzureFunctionsConfigurationOptions _azureFunctionsSetting;

        public AzureFunctionsClient([NotNull] AzureFunctionsConfigurationOptions azureFunctionsSetting, [NotNull] IHttpClientFactory clientFactory)
        {
            _azureFunctionsSetting = azureFunctionsSetting ?? throw new ArgumentNullException(nameof(azureFunctionsSetting));
            _clientFactory = clientFactory;
        }
        
        public async Task<(decimal? Weight, int? Quantity)> RequestBinMeasureAsync(int binId, decimal? referenceWeight, CancellationToken cancellationToken)
        {
            if (binId <= 0)
            {
                throw new Exception("Bin identifier must be greater than 0.");
            }

            if (binId <= 0)
            {
                throw new Exception("Request timeout must be greater than 0.");
            }

            using HttpClient client = _clientFactory.CreateClient("Par.CommandCenter.CloudDeviceManagement.ScaleRequestClient");
            // Set the request timeout
            client.Timeout = TimeSpan.FromSeconds(60);


            var measureRequestUrl = $"{_azureFunctionsSetting.RequestBinMeasureFunctionURL}?id={binId}";

            if (referenceWeight == null)
            {
                // No reference weight was specified in the request so remove the optional query string parameter that relates to it
                measureRequestUrl = measureRequestUrl.Replace("&refWeight=", string.Empty);
            }

            using var measureRequest = new HttpRequestMessage(HttpMethod.Get, measureRequestUrl);
            // Submit the request
            using var measureResponse = await client.SendAsync(measureRequest, cancellationToken);
            // Request was submitted successfully
            if (measureResponse.IsSuccessStatusCode)
            {
                RestResponse responseContent = JsonConvert.DeserializeObject<RestResponse>(await measureResponse.Content.ReadAsStringAsync());

                // Request was handled successfully
                if (responseContent.Success)
                {
                    ResponseData responseData = JsonConvert.DeserializeObject<List<ResponseData>>(responseContent.Data.ToString())[0];

                    return (responseData.Weight, responseData.Quantity);
                }
                else
                {
                    throw new Exception($"Error measuring bin {binId}: {responseContent.Message}");
                }
            }
            else
            {
                throw new Exception($"Error measuring bin {binId}: {measureResponse.ReasonPhrase} ({(int)measureResponse.StatusCode})");
            }
        }      
    }
}

