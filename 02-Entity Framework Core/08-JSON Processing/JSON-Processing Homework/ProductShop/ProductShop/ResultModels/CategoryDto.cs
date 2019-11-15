using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace ProductShop.ResultModels
{
    public class CategoryDto
    {
        [JsonProperty("category")]
        public string CategoryName { get; set; }

        [JsonProperty("productsCount")]
        public int ProductsCount { get; set; }

        [JsonProperty("averagePrice")]
        public string AveragePrice { get; set; }

        [JsonProperty("totalRevenue")]
        public string TotalRevenue { get; set; }
    }
}
