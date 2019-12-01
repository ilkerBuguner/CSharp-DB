using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace VaporStore.DataProcessor.Import
{
    public class ImportGamesDto
    {
        [Required]
        [JsonProperty("Name")]
        public string Name { get; set; }

        [Range(0, double.MaxValue), Required]
        [JsonProperty("Price")]
        public decimal Price { get; set; }

        [Required]
        [JsonProperty("ReleaseDate")]
        public string ReleaseDate { get; set; }

        [Required]
        [JsonProperty("Developer")]
        public string Developer { get; set; }

        [Required]
        [JsonProperty("Genre")]
        public string Genre { get; set; }

        [MinLength(1)]
        [JsonProperty("Tags")]
        public ICollection<string> Tags { get; set; }
    }
}
