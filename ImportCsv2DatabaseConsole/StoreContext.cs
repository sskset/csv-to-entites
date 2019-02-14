using Entities;
using System.Data.Entity;

namespace ImportCsv2DatabaseConsole
{
    public class StoreContext : DbContext
    {
        public StoreContext() : base("name=StoreContext")
        {

        }

        public DbSet<Product> Products { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<OrderItem> OrderItems { get; set; }
        public DbSet<Customer> Customers { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Order>()
                .HasRequired(x => x.Customer)
                .WithMany(x => x.Orders)
                .HasForeignKey(x => x.CustomerId);

            modelBuilder.Entity<Customer>()
                .HasMany(c => c.Orders)
                .WithRequired(o => o.Customer);

            modelBuilder.Entity<Order>()
                .HasMany(x => x.OrderItems)
                .WithRequired(x => x.Order);

            modelBuilder.Entity<OrderItem>()
                .HasRequired(x => x.Order)
                .WithMany(o => o.OrderItems)
                .HasForeignKey(x => x.OrderId);
        }
    }

    public class StoreDbInitializer : CreateDatabaseIfNotExists<StoreContext>
    {
        protected override void Seed(StoreContext context)
        {
            base.Seed(context);
        }
    }
}
