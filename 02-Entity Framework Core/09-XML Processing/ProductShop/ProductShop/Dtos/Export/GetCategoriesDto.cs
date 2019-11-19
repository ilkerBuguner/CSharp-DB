using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Serialization;

namespace ProductShop.Dtos.Export
{
    [XmlType("Category")]
    public class GetCategoriesDto
    {
        [XmlElement(ElementName = "name")]
        public string Name { get; set; }

        [XmlElement(ElementName = "count")]
        public int Count { get; set; }

        [XmlElement(ElementName = "averagePrice")]
        public decimal? AveragePrice { get; set; }

        [XmlElement(ElementName = "totalRevenue")]
        public decimal? TotalRevenue { get; set; }

    }
}
