namespace MusicHub.DataProcessor
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.IO;
    using System.Linq;
    using System.Text;
    using System.Xml.Serialization;
    using AutoMapper;
    using Data;
    using MusicHub.Data.Models;
    using MusicHub.Data.Models.Enums;
    using MusicHub.DataProcessor.ImportDtos;
    using Newtonsoft.Json;
    using ValidationContext = System.ComponentModel.DataAnnotations.ValidationContext;

    public class Deserializer
    {
        private const string ErrorMessage = "Invalid data";

        private const string SuccessfullyImportedWriter
            = "Imported {0}";
        private const string SuccessfullyImportedProducerWithPhone
            = "Imported {0} with phone: {1} produces {2} albums";
        private const string SuccessfullyImportedProducerWithNoPhone
            = "Imported {0} with no phone number produces {1} albums";
        private const string SuccessfullyImportedSong
            = "Imported {0} ({1} genre) with duration {2}";
        private const string SuccessfullyImportedPerformer
            = "Imported {0} ({1} songs)";

        public static string ImportWriters(MusicHubDbContext context, string jsonString)
        {
            var writerDtos = JsonConvert.DeserializeObject<ImportWriterDto[]>(jsonString);

            List<Writer> writers = new List<Writer>();

            StringBuilder sb = new StringBuilder();

            foreach (var writerDto in writerDtos)
            {
                var writer = Mapper.Map<Writer>(writerDto);

                bool isWriterValid = IsValid(writer);

                if (isWriterValid == false)
                {
                    sb.AppendLine(ErrorMessage);
                    continue;
                }

                writers.Add(writer);
                sb.AppendLine($"Imported {writer.Name}");
            }

            context.AddRange(writers);
            context.SaveChanges();

            return sb.ToString().TrimEnd();
        }

        public static string ImportProducersAlbums(MusicHubDbContext context, string jsonString)
        {
            var producerDtos = JsonConvert.DeserializeObject<ImportProducerDto[]>(jsonString);

            var producers = new List<Producer>();

            StringBuilder sb = new StringBuilder();

            foreach (var producerDto in producerDtos)
            {
                var producer = Mapper.Map<Producer>(producerDto);
                bool isProducerValid = IsValid(producer);
                bool isAlbumsNotValid = producerDto.Albums.Any(a => IsValid(a) == false);

                if (isProducerValid == false || isAlbumsNotValid == true)
                {
                    sb.AppendLine(ErrorMessage);
                    continue;
                }

                foreach (var albumDto in producerDto.Albums)
                {
                    var album = Mapper.Map<Album>(albumDto);
                    producer.Albums.Add(album);
                }

                if (producer.PhoneNumber != null)
                {
                    sb.AppendLine(String.Format(SuccessfullyImportedProducerWithPhone,
                        producer.Name,
                        producer.PhoneNumber,
                        producer.Albums.Count));
                }
                else
                {
                    sb.AppendLine(String.Format(SuccessfullyImportedProducerWithNoPhone,
                        producer.Name,
                        producer.Albums.Count));
                }

                producers.Add(producer);
            }

            context.Producers.AddRange(producers);
            context.SaveChanges();
            return sb.ToString().TrimEnd();
        }

        public static string ImportSongs(MusicHubDbContext context, string xmlString)
        {
            var serializer = new XmlSerializer(typeof(ImportSongDto[]), new XmlRootAttribute("Songs"));
            var songsDtos = (ImportSongDto[])serializer.Deserialize(new StringReader(xmlString));

            var songs = new List<Song>();

            StringBuilder sb = new StringBuilder();

            foreach (var songDto in songsDtos)
            {
                bool isValidGenre = Enum.IsDefined(typeof(Genre), songDto.Genre);
                bool isValidAlbum = context.Albums.Any(a => a.Id == songDto.AlbumId);
                bool isValidWriter = context.Writers.Any(w => w.Id == songDto.WriterId);

                if (isValidGenre == false || isValidAlbum == false || isValidWriter == false)
                {
                    sb.AppendLine(ErrorMessage);
                    continue;
                }

                var song = Mapper.Map<Song>(songDto);
                bool isValidSong = IsValid(song);

                if (isValidSong == false)
                {
                    sb.AppendLine(ErrorMessage);
                    continue;
                }

                songs.Add(song);

                sb.AppendLine(String.Format(SuccessfullyImportedSong,
                    song.Name,
                    song.Genre.ToString(),
                    song.Duration.ToString(@"hh\:mm\:ss")));
            }

            context.Songs.AddRange(songs);
            context.SaveChanges();

            return sb.ToString().TrimEnd();
        }

        public static string ImportSongPerformers(MusicHubDbContext context, string xmlString)
        {
            var serializer = new XmlSerializer(typeof(ImportPerformerDto[]), new XmlRootAttribute("Performers"));
            var songPerformersDtos = (ImportPerformerDto[])serializer.Deserialize(new StringReader(xmlString));

            var performers = new List<Performer>();

            StringBuilder sb = new StringBuilder();

            foreach (var songPerformerDto in songPerformersDtos)
            {
                var performer = Mapper.Map<Performer>(songPerformerDto);
                bool isValidPerformer = IsValid(performer);

                var songs = context.Songs.Select(s => s.Id).ToList();
                var doesSongsExist = songPerformerDto.PerformersSongs
                    .Select(x => x.Id)
                    .All(value => songs.Contains(value));

                if (isValidPerformer == false || doesSongsExist == false)
                {
                    sb.AppendLine(ErrorMessage);
                    continue;
                }

                foreach (var songDto in songPerformerDto.PerformersSongs)
                {
                    SongPerformer songPerformer = new SongPerformer
                    {
                        Performer = performer,
                        PerformerId = performer.Id,
                        Song = context.Songs.Find(songDto.Id),
                        SongId = songDto.Id
                    };

                    performer.PerformerSongs.Add(songPerformer);
                }

                performers.Add(performer);

                sb.AppendLine(String.Format(SuccessfullyImportedPerformer
                    , performer.FirstName
                    , performer.PerformerSongs.Count));
            }

            context.Performers.AddRange(performers);
            context.SaveChanges();

            return sb.ToString().TrimEnd();

        }

        private static bool IsValid(object dto)
        {
            var validationContext = new ValidationContext(dto);
            var validationResult = new List<ValidationResult>();

            return Validator.TryValidateObject(dto, validationContext, validationResult, true);
        }

    }
}