using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace TeisterMask.DataProcessor.ImportDto
{
    public class ImportEmployeeDto
    {
        [JsonProperty("Username")]
        [MinLength(3), MaxLength(40), RegularExpression(@"^[A-Za-z0-9]+$"), Required]
        public string Username { get; set; }

        [JsonProperty("Email")]
        [Required]
        public string Email { get; set; }

        [JsonProperty("Phone")]
        [RegularExpression(@"^[0-9]{3}-[0-9]{3}-[0-9]{4}$"), Required]
        public string Phone { get; set; }

        [JsonProperty("Tasks")]
        public ICollection<int> Tasks { get; set; } = new HashSet<int>();
    }
}
