namespace Cinema.DataProcessor
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.Globalization;
    using System.IO;
    using System.Linq;
    using System.Text;
    using System.Xml.Serialization;
    using AutoMapper;
    using Cinema.Data.Models;
    using Cinema.DataProcessor.ImportDto;
    using Data;
    using Newtonsoft.Json;
    using ValidationContext = System.ComponentModel.DataAnnotations.ValidationContext;

    public class Deserializer
    {
        private const string ErrorMessage = "Invalid data!";
        private const string SuccessfulImportMovie 
            = "Successfully imported {0} with genre {1} and rating {2}!";
        private const string SuccessfulImportHallSeat 
            = "Successfully imported {0}({1}) with {2} seats!";
        private const string SuccessfulImportProjection 
            = "Successfully imported projection {0} on {1}!";
        private const string SuccessfulImportCustomerTicket 
            = "Successfully imported customer {0} {1} with bought tickets: {2}!";

        public static string ImportMovies(CinemaContext context, string jsonString)
        {
            var moviesDtos = JsonConvert.DeserializeObject<ImportMoviesDto[]>(jsonString);

            List<Movie> movies = new List<Movie>();

            StringBuilder sb = new StringBuilder();

            foreach (var movieDto in moviesDtos)
            {
                var movie = Mapper.Map<Movie>(movieDto);

                if (!IsValid(movie))
                {
                    sb.AppendLine(ErrorMessage);
                    continue;
                }

                if (context.Movies.Any(m => m.Title == movie.Title))
                {
                    sb.AppendLine(ErrorMessage);
                    continue;
                }

                sb.AppendLine(String.Format(SuccessfulImportMovie, movie.Title, movie.Genre.ToString(), movie.Rating.ToString("F2")));
                movies.Add(movie);
            }

            context.Movies.AddRange(movies);
            context.SaveChanges();

            return sb.ToString().TrimEnd();
        }

        public static string ImportHallSeats(CinemaContext context, string jsonString)
        {
            var hallsDtos = JsonConvert.DeserializeObject<ImportHallsDto[]>(jsonString);

            StringBuilder sb = new StringBuilder();

            var halls = new List<Hall>();

            foreach (var hallDto in hallsDtos)
            {
                var hall = Mapper.Map<Hall>(hallDto);

                if (!IsValid(hall) || !IsValid(hallDto))
                {
                    sb.AppendLine(ErrorMessage);
                    continue;
                }

                for (int i = 0; i < hallDto.SeatsCount; i++)
                {
                    hall.Seats.Add(new Seat());
                }

                string projectionType = string.Empty;

                if (hall.Is3D && hall.Is4Dx)
                {
                    projectionType = "4Dx/3D";
                }
                else if (hall.Is3D)
                {
                    projectionType = "3D";
                }
                else if (hall.Is4Dx)
                {
                    projectionType = "4Dx";
                }
                else
                {
                    projectionType = "Normal";
                }

                sb.AppendLine(String.Format(SuccessfulImportHallSeat, hall.Name, projectionType, hall.Seats.Count));

                halls.Add(hall);
            }

            context.Halls.AddRange(halls);
            context.SaveChanges();

            return sb.ToString().TrimEnd();
        }

        public static string ImportProjections(CinemaContext context, string xmlString)
        {
            var serializer = new XmlSerializer(typeof(ImportProjectionDto[]), new XmlRootAttribute("Projections"));
            var projectionDtos = (ImportProjectionDto[])serializer.Deserialize(new StringReader(xmlString));

            StringBuilder sb = new StringBuilder();

            var projections = new List<Projection>();

            foreach (var projectionDto in projectionDtos)
            {
                var projection = Mapper.Map<Projection>(projectionDto);

                bool isValidProjection = IsValid(projection);
                bool doesMovieExist = context.Movies.Any(x => x.Id == projection.MovieId);
                bool doesHallExist = context.Halls.Any(x => x.Id == projection.HallId);

                if (doesMovieExist == false || doesHallExist == false || isValidProjection == false)
                {
                    sb.AppendLine(ErrorMessage);
                    continue;
                }

                var movie = context.Movies.Find(projection.MovieId);
                var hall = context.Halls.Find(projection.HallId);
                projection.Movie = movie;
                projection.Hall = hall;

                projections.Add(projection);

                sb.AppendLine(String.Format(SuccessfulImportProjection,
                    projection.Movie.Title,
                    projection.DateTime.ToString("MM/dd/yyyy")));
            }

            context.Projections.AddRange(projections);
            context.SaveChanges();

            Console.WriteLine(projections.Count);

            return sb.ToString().TrimEnd();
        }

        public static string ImportCustomerTickets(CinemaContext context, string xmlString)
        {
            var serializer = new XmlSerializer(typeof(ImportCustomerDto[]), new XmlRootAttribute("Customers"));
            var customerDtos = (ImportCustomerDto[])serializer.Deserialize(new StringReader(xmlString));

            StringBuilder sb = new StringBuilder();
            var customers = new List<Customer>();

            foreach (var customerDto in customerDtos)
            {
                var customer = Mapper.Map<Customer>(customerDto);

                
                var projectionIds = context.Projections.Select(p => p.Id).ToList();
                var areAllProjectionsValid = customerDto.CustomerTickets.Select(t => t.ProjectionId)
                    .All(t => projectionIds.Contains(t));

                if (!IsValid(customer) || areAllProjectionsValid == false)
                {
                    sb.AppendLine(ErrorMessage);
                    continue;
                }

                foreach (var ticketDto in customerDto.CustomerTickets)
                {
                    var ticket = Mapper.Map<Ticket>(ticketDto);

                    customer.Tickets.Add(ticket);
                }

                customers.Add(customer);

                sb.AppendLine(String.Format(SuccessfulImportCustomerTicket,
                    customer.FirstName,
                    customer.LastName,
                    customer.Tickets.Count));
            }

            context.Customers.AddRange(customers);
            context.SaveChanges();

            return sb.ToString().TrimEnd();
        }

        public static bool IsValid(object obj)
        {
            var validationContext = new ValidationContext(obj);
            var validationResult = new List<ValidationResult>();

            return Validator.TryValidateObject(obj, validationContext, validationResult, true);
        }
    }
}