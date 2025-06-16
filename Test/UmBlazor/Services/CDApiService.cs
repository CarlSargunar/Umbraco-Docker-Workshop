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
                        Image = $"{imageBaseUrl}{item.properties.photos[0].url}?width=200" 
                    };
                    products.Add(product);
                }
            }

            return products;
        }

    }
}
