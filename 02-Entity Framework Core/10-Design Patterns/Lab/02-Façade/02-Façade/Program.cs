using _02_Façade.Models;
using System;

namespace _02_Façade
{
    class Program
    {
        static void Main(string[] args)
        {
            var car = new CarBuilderFacade()
                .Info
                .WithType("BWM")
                .WithColor("Black")
                .WithNumberOfDoors(5)
                .Built
                .InCity("Novi Pazar")
                .AtAddress("Perfect address")
                .Build();

            Console.WriteLine(car);
        }
    }
}
