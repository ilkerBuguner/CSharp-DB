using _03_Command_Pattern.Interfaces;
using _03_Command_Pattern.Models;
using _03_Command_Pattern.Models.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace _03_Command_Pattern.Commands
{
    public class ProductCommand : ICommand
    {
        private readonly Product product;
        private readonly PriceAction priceAction;
        private readonly int amount;

        public ProductCommand(Product product, PriceAction priceAction, int amount)
        {
            this.product = product;
            this.priceAction = priceAction;
            this.amount = amount;
        }

        public void ExecuteAction()
        {
            if (priceAction == PriceAction.Increase)
            {
                product.IncreasePrice(amount);
            }
            else
            {
                product.DecreasePrice(amount);
            }
        }
    }
}
