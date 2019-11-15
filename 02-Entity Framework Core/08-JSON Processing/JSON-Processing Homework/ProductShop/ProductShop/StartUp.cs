using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Newtonsoft.Json;
using ProductShop.Data;
using ProductShop.Models;
using ProductShop.ResultModels;

namespace ProductShop
{
    public class StartUp
    {
        public static void Main(string[] args)
        {
            var context = new ProductShopContext();
            //context.Database.EnsureCreated();

            var file = File.ReadAllText(@"C:\Users\Acer\source\repos\08-JSON Processing\JSON-Processing Homework\ProductShop\ProductShop\Datasets\categories-products.json");

            Console.WriteLine(GetCategoriesByProductsCount(context));
        }

        //01-Import Users
        public static string ImportUsers(ProductShopContext context, string inputJson)
        {
            var users = JsonConvert.DeserializeObject<User[]>(inputJson);
            context.Users.AddRange(users);
            var count = context.SaveChanges();

            return $"Successfully imported {count}";
        }

        //02-Import Products
        public static string ImportProducts(ProductShopContext context, string inputJson)
        {
            var products = JsonConvert.DeserializeObject<Product[]>(inputJson);
            context.Products.AddRange(products);
            var count = context.SaveChanges();

            return $"Successfully imported {count}";
        }

        //03-Import Categories
        public static string ImportCategories(ProductShopContext context, string inputJson)
        {
            var categories = JsonConvert.DeserializeObject<Category[]>(inputJson)
                .Where(c => c.Name != null);
            context.Categories.AddRange(categories);
            var count = context.SaveChanges();

            return $"Successfully imported {count}";
        }

        //04-Import Categories and Products
        public static string ImportCategoryProducts(ProductShopContext context, string inputJson)
        {
            var categoriesProducts = JsonConvert.DeserializeObject<CategoryProduct[]>(inputJson);
            context.CategoryProducts.AddRange(categoriesProducts);
            var count = context.SaveChanges();

            return $"Successfully imported {count}";
        }

        //05-Export Products In Range
        public static string GetProductsInRange(ProductShopContext context)
        {
            var products = context.Products
                .Select(p => new ProductInRangeDto()
                {
                    ProductName = p.Name,
                    Price = p.Price,
                    SellerFullName = $"{p.Seller.FirstName} {p.Seller.LastName}"
                })
                .Where(p => p.Price >= 500 && p.Price <= 1000)
                .OrderBy(p => p.Price)
                .ToArray();

            var resultJson = JsonConvert.SerializeObject(products, Formatting.Indented);

            return resultJson;
        }

        //06-Export Sold Products
        public static string GetSoldProducts(ProductShopContext context)
        {
            var users = context.Users
                .Where(u => u.ProductsSold.Any(ps => ps.Buyer != null))
                .OrderBy(u => u.LastName)
                .ThenBy(u => u.FirstName)
                .Select(u => new UserSoldProductsDto()
                {
                    FirstName = u.FirstName,
                    LastName = u.LastName,
                    SoldProducts = u.ProductsSold
                    .Where(p => p.Buyer != null)
                    .Select(p => new SoldProductDto()
                    {
                        Name = p.Name,
                        Price = p.Price,
                        BuyerFirstName = p.Buyer.FirstName,
                        BuyerLastName = p.Buyer.LastName
                    })
                    .ToList()
                })
                .ToList();

            var resultJson = JsonConvert.SerializeObject(users, Formatting.Indented);

            return resultJson;
        }

        //7-Export Categories By Products Count
        public static string GetCategoriesByProductsCount(ProductShopContext context)
        {
            var categories = context.Categories
                .OrderByDescending(c => c.CategoryProducts.Count())
                .Select(c => new CategoryDto()
                {
                    CategoryName = c.Name,
                    ProductsCount = c.CategoryProducts.Count(),
                    AveragePrice = $@"{c.CategoryProducts.Sum(x => x.Product.Price) / c.CategoryProducts.Count():F2}",
                    TotalRevenue = $"{c.CategoryProducts.Sum(x => x.Product.Price):F2}"
                })
                .ToList();

            var resultJson = JsonConvert.SerializeObject(categories, Formatting.Indented);

            return resultJson;
        }

        //08-Export Users and Products
    }
}