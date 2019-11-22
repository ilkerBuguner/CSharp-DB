using _01_Singleton.Models;
using System;

namespace _01_Singleton
{
    class Program
    {
        static void Main(string[] args)
        {
            var db = SingletonDataContainer.Instance;
            Console.WriteLine(db.GetPopulation("Sofia"));
            var db2 = SingletonDataContainer.Instance;
            Console.WriteLine(db.GetPopulation("Ankara"));
        }
    }
}
