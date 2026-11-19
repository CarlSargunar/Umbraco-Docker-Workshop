using System.Net.Http.Json;
using UmBlazor.Services.ContentDelivery;

namespace UmBlazor.Services
{
    public class CDApiService
    {

        private readonly HttpClient _httpClient;
        private readonly IConfiguration _configuration;

        public CDApiService(HttpClient httpClient, IConfiguration configuration)
        {
            _httpClient = httpClient;
            _configuration = configuration;
        }


        public async Task<List<Product>> GetProducts()
        {
            // Load Products from from the Content Delivery API
            var productList = await FetchProductsFromContentDeliveryApi();
            //var productList = await FetchLocalProducts();
            return productList;
        }

        private async Task<List<Product>> FetchProductsFromContentDeliveryApi()
        {
            var products = new List<Product>();
            var apiResponse = await _httpClient.GetAsync(_configuration["UmbracoURL"]);
            var imageBaseUrl = _configuration["ImageBaseURL"];
            // Throw if the API returns an error (e.g. 404 or 500), so the page can show it
            apiResponse.EnsureSuccessStatusCode();
            if (apiResponse.IsSuccessStatusCode)
            {
                var contentDeliveryResponse = await apiResponse.Content.ReadFromJsonAsync<ContentDeliveryResponse>();

                foreach (var item in contentDeliveryResponse.items)
                {
                    var product = new Product
                    {
                        Name = item.name,
                        Price = item.properties.price,
                        Sku = item.properties.sku,
                        Description = item.properties.description,
                        Category = item.properties.category,
                        // Umbraco 17 requires image processing URLs (e.g. ?width=200) to be signed, which
                        // the Delivery API URLs are not, so we load the original image and size it with CSS
                        Image = $"{imageBaseUrl}{item.properties.photos[0].url}"
                    };
                    products.Add(product);
                }
            }

            return products;
        }

    }
}
