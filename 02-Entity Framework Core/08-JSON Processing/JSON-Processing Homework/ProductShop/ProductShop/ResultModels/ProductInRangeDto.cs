using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace ProductShop.ResultModels
{
    public class ProductInRangeDto
    {
        [JsonProperty("name")]
        public string ProductName { get; set; }

        [JsonProperty("price")]
        public decimal Price { get; set; }

        [JsonProperty("seller")]
        public string SellerFullName { get; set; }
    }
}
