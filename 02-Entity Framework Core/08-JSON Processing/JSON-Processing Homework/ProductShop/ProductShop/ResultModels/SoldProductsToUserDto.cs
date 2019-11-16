using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace ProductShop.ResultModels
{
    public class SoldProductsToUserDto
    {
        [JsonProperty("count")]
        public int Count { get; set; }

        [JsonProperty("products")]
        public ICollection<SoldProductsDto> Products { get; set; }
    }
}
