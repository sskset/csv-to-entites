using Core;
using Entities;
using System;
using System.IO;
using System.Linq;

namespace ImportCsv2DatabaseConsole
{
    class Program
    {
        static void Main(string[] args)
        {
            Import();

            QueryNDisplay();

            Console.ReadKey();
        }

        static void Import()
        {
            var productCsvFile = Path.Combine(Environment.CurrentDirectory, "product.csv");
            using (var storeCtx = new StoreContext())
            {
                var products = CsvLoader.Load<Product>(productCsvFile);
                storeCtx.Database.ExecuteSqlCommand("DELETE [Products]");
                storeCtx.Products.AddRange(products);
                storeCtx.SaveChanges();
            }
        }

        static void QueryNDisplay()
        {
            using (var storeCtx = new StoreContext())
            {
                var products = storeCtx.Products.ToList();

                foreach (var @product in products)
                {
                    Console.WriteLine(product);
                }
            }
        }
    }
}
