using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace ProductShop.ResultModels
{
    public class UsersAndProductsDto
    {
        [JsonProperty("usersCount")]
        public int UsersCount { get; set; }

        [JsonProperty("users")]
        public ICollection<UserWithProductsDto> Users { get; set; }
    }
}
