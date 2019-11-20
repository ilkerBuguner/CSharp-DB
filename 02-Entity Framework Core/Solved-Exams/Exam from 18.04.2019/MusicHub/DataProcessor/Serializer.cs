namespace MusicHub.DataProcessor
{
    using System;
    using System.IO;
    using System.Linq;
    using System.Text;
    using System.Xml.Serialization;
    using Data;
    using MusicHub.DataProcessor.ExportDtos;
    using Newtonsoft.Json;

    public class Serializer
    {
        public static string ExportAlbumsInfo(MusicHubDbContext context, int producerId)
        {
            var albums = context.Albums
                .Where(a => a.ProducerId == producerId)
                .OrderByDescending(a => a.Songs.Sum(s => s.Price))
                .Select(a => new ExportAlbumDto
                {
                    AlbumName = a.Name,
                    ReleaseDate = a.ReleaseDate.ToString("MM/dd/yyyy"),
                    ProducerName = a.Producer.Name,
                    Songs = a.Songs.Select(s => new ExportSongForAlbumDto
                    {
                        SongName = s.Name,
                        Price = $"{s.Price:F2}",
                        Writer = s.Writer.Name
                    })
                    .OrderByDescending(s => s.SongName)
                    .ThenBy(s => s.Writer)
                    .ToArray(),
                    AlbumPrice = a.Songs.Sum(s => s.Price).ToString("F2")
                })
                .ToArray();

            var serializer = JsonConvert.SerializeObject(albums, Formatting.Indented);

            return serializer;


            throw new NotImplementedException();
        }

        public static string ExportSongsAboveDuration(MusicHubDbContext context, int duration)
        {
            var songs = context.Songs
                .Where(s => s.Duration.TotalSeconds > duration)
                .OrderBy(s => s.Name)
                .ThenBy(s => s.Writer.Name)
                .ThenBy(s => s.SongPerformers
                        .Select(p => $"{p.Performer.FirstName} {p.Performer.LastName}")
                        .FirstOrDefault())
                .Select(s => new SongAboveDurationDto
                {
                    SongName = s.Name,
                    Writer = s.Writer.Name,
                    Performer = s.SongPerformers
                        .Select(p => $"{p.Performer.FirstName} {p.Performer.LastName}")
                        .FirstOrDefault(),
                    AlbumProducer = s.Album.Producer.Name,
                    Duration = s.Duration.ToString(@"hh\:mm\:ss")
                })
                .ToArray();


            StringBuilder sb = new StringBuilder();

            var namespaces = new XmlSerializerNamespaces();
            namespaces.Add("", "");

            var serializer = new XmlSerializer(typeof(SongAboveDurationDto[]), new XmlRootAttribute("Songs"));

            serializer.Serialize(new StringWriter(sb), songs, namespaces);

            return sb.ToString().TrimEnd();
        }
    }
}