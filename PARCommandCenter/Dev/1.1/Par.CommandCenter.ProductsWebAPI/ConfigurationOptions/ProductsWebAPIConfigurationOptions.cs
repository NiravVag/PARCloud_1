using System.Collections.Generic;

namespace Par.CommandCenter.ProductsWebAPI.ConfigurationOptions
{
    public class ProductsWebAPIConfigurationOptions
    {
        public string BaseUrl { get; set; }

        public string ClientSecret { get; set; }

        public string ClientId { get; set; }

        public string Authority { get; set; }

        public string RedirectUri { get; set; }

        public IEnumerable<string> Scopes { get; set; }
    }
}
