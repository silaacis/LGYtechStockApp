using LGYtechStockApp.Models;
using System.Data.Entity;

namespace LGYtechStockApp.Data
{
    public class StockDbContext : DbContext
    {
        public StockDbContext() : base("name=StockDbContext")
        {

        }
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Product>()
                .HasIndex(p => p.Sku)
                .IsUnique();

            modelBuilder.Entity<Product>()
                .Property(p => p.Price)
                .HasPrecision(18, 2);

            modelBuilder.Entity<OrderLine>()
                .Property(p => p.UnitPrice)
                .HasPrecision(18, 2);

            base.OnModelCreating(modelBuilder);
        }

        public DbSet<Product> Products { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<OrderLine> OrderLines { get; set; }
        public DbSet<StockMovement> StockMovements { get; set; }
    }
}