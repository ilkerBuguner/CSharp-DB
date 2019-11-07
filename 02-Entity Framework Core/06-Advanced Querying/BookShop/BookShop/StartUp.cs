namespace BookShop
{
    using Data;
    using Initializer;
    using System.Linq;
    using System.Text;
    using System;
    using BookShop.Models.Enums;
    using System.Collections.Generic;
    using System.Globalization;

    public class StartUp
    {
        public static void Main()
        {
            using (var db = new BookShopContext())
            {
                DbInitializer.ResetDatabase(db);


                //ChangeTheMethodHereToSwitchBetweenExercises!
                Console.WriteLine(GetGoldenBooks(db));
            }
        }

        //01-Age Restriction
        public static string GetBooksByAgeRestriction(BookShopContext context, string command)
        {
            var age = Enum.Parse<AgeRestriction>(command, true);

            var books = context.Books
                .Select(b => new
                {
                    b.Title,
                    b.AgeRestriction
                })
                .Where(b => b.AgeRestriction == age)
                .OrderBy(x => x.Title)
                .ToList();

            StringBuilder sb = new StringBuilder();

            foreach (var book in books)
            {
                sb.AppendLine(book.Title);
            }

            return sb.ToString().TrimEnd();
        }

        //02-Golden Books
        public static string GetGoldenBooks(BookShopContext context)
        {
            var books = context.Books
                .Select(b => new
                {
                    EditionType = b.EditionType,
                    Title = b.Title,
                    Copies = b.Copies,
                    Id = b.BookId
                })
                .Where(b => b.Copies < 5000 && b.EditionType == EditionType.Gold)
                .OrderBy(b => b.Id)
                .ToList();

            StringBuilder sb = new StringBuilder();

            foreach (var book in books)
            {
                sb.AppendLine(book.Title);
            }

            return sb.ToString().TrimEnd();
        }

        //03-Books by Price
        public static string GetBooksByPrice(BookShopContext context)
        {
            var books = context.Books
                .Select(b => new
                {
                    Title = b.Title,
                    Price = b.Price
                })
                .Where(b => b.Price > 40)
                .OrderByDescending(b => b.Price)
                .ToList();

            StringBuilder sb = new StringBuilder();

            foreach (var book in books)
            {
                sb.AppendLine($"{book.Title} - ${book.Price:F2}");
            }

            return sb.ToString().TrimEnd();
        }

        //04-Not Released In
        public static string GetBooksNotReleasedIn(BookShopContext context, int year)
        {
            var books = context.Books
                .Select(b => new
                {
                    Id = b.BookId,
                    Title = b.Title,
                    Year = b.ReleaseDate.Value.Year
                })
                .Where(b => b.Year < year || b.Year > year)
                .OrderBy(b => b.Id)
                .ToList();

            StringBuilder sb = new StringBuilder();

            foreach (var book in books)
            {
                sb.AppendLine(book.Title);
            }

            return sb.ToString().TrimEnd();
        }

        //05-Book Titles by Category
        public static string GetBooksByCategory(BookShopContext context, string input)
        {
            string[] categories = input
                .Split(" ", StringSplitOptions.RemoveEmptyEntries)
                .ToArray();

            List<string> titles = new List<string>();

            foreach (var category in categories)
            {
                var books = context.Books
                    .Where(b => b.BookCategories
                    .Any(bc => bc.Category.Name.ToLower() == category.ToLower()))
                .ToList();

                foreach (var book in books)
                {
                    titles.Add(book.Title);
                }
            }

            StringBuilder sb = new StringBuilder();

            foreach (var bookTitle in titles.OrderBy(x => x))
            {
                sb.AppendLine(bookTitle);
            }

            return sb.ToString().TrimEnd();
            
        }

        //06-Released Before Date
        public static string GetBooksReleasedBefore(BookShopContext context, string date)
        {
            var dateTime = DateTime.ParseExact(date, "dd-MM-yyyy", CultureInfo.InvariantCulture);

            var books = context.Books
                .Select(b => new
                {
                    Title = b.Title,
                    EditionType = b.EditionType.ToString(),
                    ReleaseDate = b.ReleaseDate,
                    Price = b.Price
                })
                .Where(b => b.ReleaseDate < dateTime)
                .OrderByDescending(b => b.ReleaseDate);

            StringBuilder sb = new StringBuilder();

            foreach (var book in books)
            {
                sb.AppendLine($"{book.Title} - {book.EditionType} - {book.Price:F2}");
            }

            return sb.ToString().TrimEnd();
        }

        //07-Author Search
        public static string GetAuthorNamesEndingIn(BookShopContext context, string input)
        {
            var authors = context.Authors
                .Select(a => new
                {
                    FullName = a.FirstName + " " + a.LastName,
                    FirstName = a.FirstName
                })
                .Where(a => a.FirstName.EndsWith(input))
                .OrderBy(a => a.FirstName)
                .ToList();

            StringBuilder sb = new StringBuilder();

            foreach (var author in authors)
            {
                sb.AppendLine(author.FullName);
            }

            return sb.ToString().TrimEnd();
        }

        //08-Book Search
        public static string GetBookTitlesContaining(BookShopContext context, string input)
        {
            var nonCaseSensitiveInput = input.ToLower();

            var books = context.Books
                .Where(x => x.Title.ToLower().Contains(nonCaseSensitiveInput))
                .OrderBy(x => x.Title)
                .Select(x => x.Title)
                .ToList();

            StringBuilder sb = new StringBuilder();

            foreach (var book in books)
            {
                sb.AppendLine(book);
            }

            return sb.ToString().TrimEnd();
        }

        //09-Book Search by Author
        public static string GetBooksByAuthor(BookShopContext context, string input)
        {
            var bookTitlesAndTheirAuthor = context.Books
                .Select(b => new
                {
                    Id = b.BookId,
                    Title = b.Title,
                    AuthorLastName = b.Author.LastName,
                    AuthorFullName = b.Author.FirstName + " " + b.Author.LastName
                })
                .Where(b => b.AuthorLastName.ToLower().StartsWith(input.ToLower()))
                .OrderBy(b => b.Id)
                .ToList();

            StringBuilder sb = new StringBuilder();

            foreach (var b in bookTitlesAndTheirAuthor)
            {
                sb.AppendLine($"{b.Title} ({b.AuthorFullName})");
            }

            return sb.ToString().TrimEnd();
        }

        //10-Count Books
        public static int CountBooks(BookShopContext context, int lengthCheck)
        {
            var bookTitles = context.Books
                .Select(b => b.Title)
                .ToList();

            int counter = 0;

            foreach (var title in bookTitles)
            {
                if (title.Length > lengthCheck)
                {
                    counter++;
                }
            }

            return counter;
        }

        //11-Total Book Copies
        public static string CountCopiesByAuthor(BookShopContext context)
        {
            var authors = context.Authors
                .Select(a => new
                {
                    FullName = a.FirstName + " " + a.LastName,
                    TotalCopies = a.Books.Sum(c => c.Copies)
                })
                .OrderByDescending(a => a.TotalCopies)
                .ToList();

            StringBuilder sb = new StringBuilder();

            foreach (var author in authors)
            {
                sb.AppendLine($"{author.FullName} - {author.TotalCopies}");
            }

            return sb.ToString().TrimEnd();
        }

        //12-Profit by Category
        public static string GetTotalProfitByCategory(BookShopContext context)
        {
            var categoriesWithTheirProfits = context.Categories
                .Select(c => new
                {
                    CategoryName = c.Name,
                    ProfitOfCurrentCategory = c.CategoryBooks
                    .Sum(p => p.Book.Copies * p.Book.Price)
                })
                .OrderByDescending(c => c.ProfitOfCurrentCategory)
                .ThenBy(c => c.CategoryName)
                .ToList();

            StringBuilder sb = new StringBuilder();

            foreach (var category in categoriesWithTheirProfits)
            {
                sb.AppendLine($"{category.CategoryName} ${category.ProfitOfCurrentCategory:F2}");
            }

            return sb.ToString().TrimEnd();
        }

        //13-Most Recent Books
        public static string GetMostRecentBooks(BookShopContext context)
        {
            var categoriesWithRecentBooks = context.Categories
                .Select(c => new
                {
                    CategoryName = c.Name,
                    MostRecentBooks = c.CategoryBooks
                    .Select(b => new
                    {
                        BookName = b.Book.Title,
                        ReleaseDate = b.Book.ReleaseDate
                    })
                    .OrderByDescending(b => b.ReleaseDate)
                    .Take(3)
                })
                .OrderBy(c => c.CategoryName);

            StringBuilder sb = new StringBuilder();

            foreach (var category in categoriesWithRecentBooks)
            {
                sb.AppendLine($"--{category.CategoryName}");
                foreach (var book in category.MostRecentBooks)
                {
                    sb.AppendLine($"{book.BookName} ({book.ReleaseDate.Value.Year})");
                }
            }

            return sb.ToString().TrimEnd();
        }

        //14-Increase Prices
        public static void IncreasePrices(BookShopContext context)
        {
            var books = context.Books
                 .Where(b => b.ReleaseDate.Value.Year < 2010);

            foreach (var book in books)
            {
                book.Price += 5;
            }

            context.SaveChanges();

        }

        //15-Remove Books
        public static int RemoveBooks(BookShopContext context)
        {
            var books = context.Books
                .Where(b => b.Copies < 4200);

            var countOfBooksToRemove = books.Count();

            context.RemoveRange(books);

            context.SaveChanges();

            return countOfBooksToRemove;
        }

    }
}
