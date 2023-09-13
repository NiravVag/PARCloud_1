using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Par.CommandCenter.Application.Interfaces;
using Par.CommandCenter.AzureMapAPI.ConfigurationOptions;

namespace Par.CommandCenter.AzureMapAPI
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddAzureMapAPIService(this IServiceCollection services, IConfiguration configuration)
        {
            var azureMapAPIConfigurationOptions = configuration.GetSection("AzureMapAPIConfigurationOptions").Get<AzureMapAPIConfigurationOptions>();

            services.AddSingleton<AzureMapAPIConfigurationOptions>(azureMapAPIConfigurationOptions);

            services.AddTransient<IAzureMapAPIWebClient, AzureMapAPIWebClient>();

            services.AddHttpClient();

            return services;
        }
    }
}
