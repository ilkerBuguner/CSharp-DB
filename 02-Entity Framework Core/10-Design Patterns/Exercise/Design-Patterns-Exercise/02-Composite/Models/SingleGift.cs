using System;
using System.Collections.Generic;
using System.Text;

namespace _01_Composite.Models
{
    public class SingleGift : GiftBase
    {
        public SingleGift(string name, int price) 
            : base(name, price)
        {

        }

        public override int CalculateTotalPrice()
        {
            Console.WriteLine($"{name} with the price {price}");

            return price;
        }
    }
}
