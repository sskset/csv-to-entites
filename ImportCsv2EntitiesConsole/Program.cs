using Core;
using Entities;
using System;
using System.IO;

namespace ImportCsv2EntitiesConsole
{
    class Program
    {
        static void Main(string[] args)
        {
            var csvFilePath = Path.Combine(Environment.CurrentDirectory, "test.csv");
            var products = CsvLoader.Load<Product>(csvFilePath);

            foreach(var product in products)
            {
                Console.WriteLine(product.ToString());
            }

            Console.ReadKey();
        }
    }
}
