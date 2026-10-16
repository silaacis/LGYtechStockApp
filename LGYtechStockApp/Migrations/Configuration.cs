namespace LGYtechStockApp.Migrations
{
    using LGYtechStockApp.Models;
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;

    internal sealed class Configuration : DbMigrationsConfiguration<LGYtechStockApp.Data.StockDbContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
        }

        protected override void Seed(LGYtechStockApp.Data.StockDbContext context)
        {
            context.Products.AddOrUpdate(
                p => p.Sku,
                new Product
                {
                    Name = "Kalem",
                    Sku = "Urun-001",
                    Price = 10.0m,
                    Stock = 100
                },
                new Product
                 {
                     Name = "Defter",
                     Sku = "URUN-002",
                     Price = 35.00m,
                     Stock = 50
                },
                new Product
                {
                    Name = "Silgi",
                    Sku = "URUN-003",
                    Price = 5.00m,
                    Stock = 3
                },
                new Product
                {
                    Name = "Cetvel",
                    Sku = "URUN-004",
                    Price = 15.00m,
                    Stock = 1
                },
                new Product
                {
                    Name = "Çanta",
                    Sku = "URUN-005",
                    Price = 450.00m,
                    Stock = 0
                }
            );
        }
    }
}
