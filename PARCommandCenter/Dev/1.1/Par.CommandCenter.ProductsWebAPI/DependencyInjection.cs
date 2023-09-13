using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Par.CommandCenter.ProductsWebAPI.ConfigurationOptions;
using Products.Web.API.Client;

using IProductsWebAPIClient = Products.Web.API.Client.IProductsWebAPIClient;

namespace Par.CommandCenter.ProductsWebAPI
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddProductsWebAPI(this IServiceCollection services, IConfiguration configuration)
        {
            var options = configuration.GetSection("ProductsWebAPIConfigurationOptions").Get<ProductsWebAPIConfigurationOptions>();

            services.AddSingleton<ProductsWebAPIConfigurationOptions>(options);

            services.AddTransient<IProductsWebAPIClient, Par.CommandCenter.ProductsWebAPI.ProductsWebAPIClient>();

            services.AddHttpClient();

            return services;
        }
    }
}
