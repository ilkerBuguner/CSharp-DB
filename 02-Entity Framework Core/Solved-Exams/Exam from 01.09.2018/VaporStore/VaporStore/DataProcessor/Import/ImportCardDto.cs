using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace VaporStore.DataProcessor.Import
{
    public class ImportCardDto
    {
        [JsonProperty("Number")]
        [RegularExpression(@"^[0-9]{4} [0-9]{4} [0-9]{4} [0-9]{4}$"), Required]
        public string Number { get; set; }

        [JsonProperty("CVC")]
        [RegularExpression(@"[0-9]{3}"), Required]
        public string Cvc { get; set; }

        [JsonProperty("Type")]
        [Required]
        public string Type { get; set; }
    }
}
