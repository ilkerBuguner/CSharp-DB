using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace MusicHub.DataProcessor.ExportDtos
{
    public class ExportSongForAlbumDto
    {
        [JsonProperty("SongName")]
        public string SongName { get; set; }

        [JsonProperty("Price")]
        public string Price { get; set; }

        [JsonProperty("Writer")]
        public string Writer { get; set; }
    }
}
