using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Par.CommandCenter.Application.Interfaces;
using Par.CommandCenter.CloudDeviceManagement.ConfigurationOptions;
using System;
using System.Net.Http;

namespace Par.CommandCenter.CloudDeviceManagement
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddCloudDeviceManagementService(this IServiceCollection services, IConfiguration configuration)
        {
            var AzureIoTHubConnectionString = configuration.GetSection("AzureIoTHubConfigurationOptions").Get<AzureIoTHubConfigurationOptions>();

            services.AddSingleton<AzureIoTHubConfigurationOptions>(AzureIoTHubConnectionString);

            services.AddTransient<IAzureIoTService, AzureIoTHubService>();


            var azureFunctionConfigurationOptions = configuration.GetSection("AzureFunctionConfigurationOptions").Get<AzureFunctionsConfigurationOptions>();

            services.AddSingleton<AzureFunctionsConfigurationOptions>(azureFunctionConfigurationOptions);

            services.AddTransient<IAzureFunctionsClient, AzureFunctionsClient>();

            services.AddHttpClient("Par.CommandCenter.CloudDeviceManagement.ScaleRequestClient");

            services.AddHttpClient("Par.CommandCenter.CloudDeviceManagement.RouterManagementClient");

            services.AddHttpClient("Par.CommandCenter.CloudDeviceManagement.RunCommandOnVmClient");

            return services;
        }
    }
}
