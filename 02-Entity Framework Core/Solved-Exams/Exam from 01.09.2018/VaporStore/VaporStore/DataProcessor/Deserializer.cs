namespace VaporStore.DataProcessor
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
    using Data;
    using Newtonsoft.Json;
    using VaporStore.Data.Models;
    using VaporStore.Data.Models.Enumerations;
    using VaporStore.DataProcessor.Import;
    using ValidationContext = System.ComponentModel.DataAnnotations.ValidationContext;

    public static class Deserializer
	{
        private const string ErrorMessage = "Invalid Data";
        private const string ImportedGame = "Added {0} ({1}) with {2} tags";
        private const string ImportedUser = "Imported {0} with {1} cards";
        private const string ImportedPurchase = "Imported {0} for {1}";

        public static string ImportGames(VaporStoreDbContext context, string jsonString)
		{
            var gameDtos = JsonConvert.DeserializeObject<ImportGamesDto[]>(jsonString);

            StringBuilder sb = new StringBuilder();

            var developers = new List<Developer>();
            var genres = new List<Genre>();
            var tags = new List<Tag>();

            var games = new List<Game>();

            foreach (var gameDto in gameDtos)
            {
                if (!IsValid(gameDto))
                {
                    sb.AppendLine(ErrorMessage);
                    continue;
                }

                var developer = developers.FirstOrDefault(x => x.Name == gameDto.Developer);

                if (developer == null)
                {
                    developer = new Developer()
                    {
                        Name = gameDto.Developer
                    };

                    developers.Add(developer);
                }

                var genre = genres.FirstOrDefault(x => x.Name == gameDto.Genre);

                if (genre == null)
                {
                    genre = new Genre()
                    {
                        Name = gameDto.Genre
                    };

                    genres.Add(genre);
                }

                var gameTags = new List<Tag>();

                foreach (var tagName in gameDto.Tags)
                {
                    var tag = tags.FirstOrDefault(x => x.Name == tagName);

                    if (tag == null)
                    {
                        tag = new Tag()
                        {
                            Name = tagName
                        };

                        tags.Add(tag);
                    }

                    gameTags.Add(tag);
                }

                Game game = new Game()
                {
                    Name = gameDto.Name,
                    Price = gameDto.Price,
                    ReleaseDate = DateTime.ParseExact(gameDto.ReleaseDate, @"yyyy-MM-dd", CultureInfo.InvariantCulture),
                    Developer = developer,
                    Genre = genre,
                    GameTags = gameTags.Select(gt => new GameTag() { Tag = gt }).ToList()
                };

                games.Add(game);
                sb.AppendLine(String.Format(ImportedGame, game.Name, game.Genre.Name, game.GameTags.Count));
            }

            context.Games.AddRange(games);
            context.SaveChanges();

            return sb.ToString().TrimEnd();
		}

		public static string ImportUsers(VaporStoreDbContext context, string jsonString)
		{
            var userDtos = JsonConvert.DeserializeObject<ImportUserDto[]>(jsonString);

            StringBuilder sb = new StringBuilder();

            var users = new List<User>();

            foreach (var userDto in userDtos)
            {
                if (!IsValid(userDto))
                {
                    sb.AppendLine(ErrorMessage);
                    continue;
                }

                var userCards = new List<Card>();

                foreach (var card in userDto.Cards)
                {
                    if (!IsValid(card))
                    {
                        sb.AppendLine(ErrorMessage);
                        continue;
                    }

                    userCards.Add(card);
                }

                var user = new User()
                {
                    FullName = userDto.FullName,
                    Username = userDto.Username,
                    Email = userDto.Email,
                    Age = userDto.Age,
                    Cards = userCards
                };

                users.Add(user);

                sb.AppendLine(String.Format(ImportedUser, user.Username, userCards.Count));
            }

            context.AddRange(users);
            context.SaveChanges();

            return sb.ToString().TrimEnd();
		}

		public static string ImportPurchases(VaporStoreDbContext context, string xmlString)
		{
            var serializer = new XmlSerializer(typeof(ImportPurchaseDto[]), new XmlRootAttribute("Purchases"));
            var purchaseDtos = (ImportPurchaseDto[])serializer.Deserialize(new StringReader(xmlString));

            var sb = new StringBuilder();

            var purchases = new List<Purchase>();

            foreach (var purchaseDto in purchaseDtos)
            {
                bool isValidPurchase = IsValid(purchaseDto);
                bool isValidType = Enum.IsDefined(typeof(PurchaseType), purchaseDto.Type);
                var targetGame = context.Games.FirstOrDefault(x => x.Name == purchaseDto.Title);
                var targetCard = context.Cards.FirstOrDefault(x => x.Number == purchaseDto.Card);

                if (isValidPurchase == false 
                    || isValidType == false
                    || targetGame == null
                    || targetCard == null)
                {
                    sb.AppendLine(ErrorMessage);
                    continue;
                }

                var purchase = new Purchase()
                {
                    Type = (PurchaseType)Enum.Parse(typeof(PurchaseType), purchaseDto.Type),
                    Card = targetCard,
                    Game = targetGame,
                    Date = DateTime.ParseExact(purchaseDto.Date, @"dd/MM/yyyy HH:mm", CultureInfo.InvariantCulture),
                    ProductKey = purchaseDto.Key
                };

                purchases.Add(purchase);

                sb.AppendLine(String.Format(ImportedPurchase, purchaseDto.Title, purchase.Card.User.Username));
            }

            context.Purchases.AddRange(purchases);
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