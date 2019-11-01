using Microsoft.EntityFrameworkCore;
using P03_SalesDatabase.Data.Models;

namespace P03_SalesDatabase.Data
{
    public class SalesContext : DbContext
    {
        public DbSet<Customer> Customers { get; set; }

        public DbSet<Product> Products { get; set; }

        public DbSet<Sale> Sales { get; set; }

        public DbSet<Store> Stores { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlServer(Configuration.DefaultConnection);
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Product>(entity =>
            {
                entity.HasKey(k => k.ProductId);

                entity.Property(p => p.Name)
                .HasMaxLength(50)
                .IsUnicode(true)
                .IsRequired(true);

                entity.Property(p => p.Description)
                .HasMaxLength(250)
                .IsRequired(false)
                .IsUnicode(true)
                .HasDefaultValue("No description");

                entity.Property(p => p.Quantity)
                .IsRequired(true);

                entity.Property(p => p.Price)
                .IsRequired(true);
            });

            modelBuilder.Entity<Customer>(entity =>
            {
                entity.HasKey(k => k.CustomerId);

                entity.Property(p => p.Name)
                .HasMaxLength(100)
                .IsUnicode(true)
                .IsRequired(true);

                entity.Property(p => p.Email)
                .HasMaxLength(80)
                .IsUnicode(true)
                .IsRequired(true);

                entity.Property(p => p.CreditCardNumber)
                .IsRequired(true);
            });

            modelBuilder.Entity<Store>(entity =>
            {
                entity.HasKey(k => k.StoreId);

                entity.Property(p => p.Name)
                .HasMaxLength(80)
                .IsUnicode(true)
                .IsRequired(true);
            });

            modelBuilder.Entity<Sale>(entity =>
            {
                entity.HasKey(k => k.SaleId);

                entity.Property(p => p.Date)
                .IsRequired(true)
                .HasColumnType("DATETIME2")
                .HasDefaultValueSql("GETDATE()");

                entity.HasOne(p => p.Product)
                .WithMany(s => s.Sales)
                .HasForeignKey(p => p.ProductId);

                entity.HasOne(c => c.Customer)
                .WithMany(s => s.Sales)
                .HasForeignKey(p => p.CustomerId);

                entity.HasOne(s => s.Store)
                .WithMany(st => st.Sales)
                .HasForeignKey(s => s.StoreId);
            });
        }
    }
}
