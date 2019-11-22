using _03_Command_Pattern.Commands;
using _03_Command_Pattern.Interfaces;
using _03_Command_Pattern.Models;
using _03_Command_Pattern.Models.Enums;
using System;

namespace _03_Command_Pattern
{
    class Program
    {
        static void Main(string[] args)
        {
            var modifyPrice = new ModifyPrice();
            var product = new Product("Phone", 500);

            Execute(product, modifyPrice, new ProductCommand(product, PriceAction.Increase, 100));

            Execute(product, modifyPrice, new ProductCommand(product, PriceAction.Increase, 50));

            Execute(product, modifyPrice, new ProductCommand(product, PriceAction.Increase, 25));

            Console.WriteLine(product);
        }

        public static void Execute(Product product, ModifyPrice modifyPrice, ICommand productCommand)
        {
            modifyPrice.SetCommand(productCommand);
            modifyPrice.Invoke();
        }
    }
}
