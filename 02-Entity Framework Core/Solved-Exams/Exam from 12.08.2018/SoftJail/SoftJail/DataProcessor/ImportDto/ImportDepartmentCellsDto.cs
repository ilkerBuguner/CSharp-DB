using Newtonsoft.Json;
using SoftJail.Data.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace SoftJail.DataProcessor.ImportDto
{
    public class ImportDepartmentCellsDto
    {
        [JsonProperty("Name")]
        public string Name { get; set; }

        [JsonProperty("Cells")]
        public ICollection<Cell> Cells { get; set; }
    }
}
