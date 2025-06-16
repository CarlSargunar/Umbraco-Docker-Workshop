using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace UmBlazor.Services
{

    public class Product
    {
        // Hard coded for demo
        private static readonly CultureInfo CultureInfo = new("en-GB");
        public string FormattedPrice => Price.ToString("C2", CultureInfo);

        public string Name { get; set; }
        public decimal Price { get; set; }
        public string Sku { get; set; }
        public string Image { get; set; }
        public string Description { get; set; }
        public string[] Category { get; set; }
    }

    [JsonSerializable(typeof(List<Product>))]
    internal sealed partial class ProductContext : JsonSerializerContext
    {

    }

}
