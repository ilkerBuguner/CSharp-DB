using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Serialization;

namespace MusicHub.DataProcessor.ImportDtos
{
    [XmlType("Performer")]
    public class ImportPerformerDto
    {
        [XmlElement("FirstName")]
        public string FirstName { get; set; }

        [XmlElement("LastName")]
        public string LastName { get; set; }

        [XmlElement("Age")]
        public int Age { get; set; }

        [XmlElement("NetWorth")]
        public decimal NetWorth { get; set; }

        [XmlArray("PerformersSongs")]
        public ImportPerformerSongDto[] PerformersSongs { get; set; }
    }
}
