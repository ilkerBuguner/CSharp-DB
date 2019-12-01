using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using VaporStore.Data.Models;

namespace VaporStore.DataProcessor.Import
{
    public class ImportUserDto
    {
        [JsonProperty("FullName")]
        [RegularExpression(@"^[A-Z]{1}[a-z]+ [A-Z]{1}[a-z]+$"), Required]
        public string FullName { get; set; }

        [JsonProperty("Username")]
        [MinLength(3), MaxLength(20), Required]
        public string Username { get; set; }

        [JsonProperty("Email")]
        [Required]
        public string Email { get; set; }

        [JsonProperty("Age")]
        [Range(3, 103), Required]
        public int Age { get; set; }

        [JsonProperty("Cards")]
        [MinLength(1)]
        public ICollection<Card> Cards { get; set; }

    }
}
