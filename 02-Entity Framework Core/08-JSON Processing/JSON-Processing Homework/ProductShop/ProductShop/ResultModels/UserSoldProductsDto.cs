using Newtonsoft.Json;
using ProductShop.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace ProductShop.ResultModels
{
    public class UserSoldProductsDto
    {
        [JsonProperty("firstName")]
        public string FirstName { get; set; }

        [JsonProperty("lastName")]
        public string LastName { get; set; }

        [JsonProperty("soldProducts")]
        public ICollection<SoldProductDto> SoldProducts { get; set; }
    }
}
