using System;
using System.Collections.Generic;
using System.Text;

namespace MusicHub.DataProcessor.ImportDtos
{
    public class ImportProducerDto
    {
        public string Name { get; set; }

        public string Pseudonym { get; set; }

        public string PhoneNumber { get; set; }

        public ICollection<ImportAlbumDto> Albums { get; set; }
    }
}
