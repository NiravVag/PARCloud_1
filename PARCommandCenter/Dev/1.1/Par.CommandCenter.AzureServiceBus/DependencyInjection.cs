using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Par.CommandCenter.Application.Interfaces;
using Par.CommandCenter.AzureServiceBus.ConfigurationOptions;
using Par.CommandCenter.AzureServiceBus.Interfaces;
using Par.CommandCenter.AzureServiceBus.Services;

namespace Par.CommandCenter.AzureServiceBus
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddAzureServiceBus(this IServiceCollection services, IConfiguration configuration)
        {
            var azureMapAPIConfigurationOptions = configuration.GetSection("AzureServiceBusConfigurationOptions").Get<AzureServiceBusConfigurationOptions>();

            services.AddSingleton(azureMapAPIConfigurationOptions);

            services.AddSingleton<IAzureServiceBusFactory, AzureServiceBusFactory>();

            services.AddTransient<IAzureServiceBusService, AzureServiceBusService>();           

            return services;
        }
    }
}
