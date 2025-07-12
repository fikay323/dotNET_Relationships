using dotNET_Relationships.Models;
using Microsoft.EntityFrameworkCore;

namespace dotNET_Relationships.Data
{
    public class AppDbContext(DbContextOptions<AppDbContext> options): DbContext(options)
    {
        public DbSet<Product> Products { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<PaymentDetail> PaymentDetails { get; set; }
        public DbSet<OrderProduct> OrderProducts { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<OrderProduct>()
                .HasKey(op => new { op.OrderId, op.ProductId, });

            modelBuilder.Entity<OrderProduct>()
                .HasOne(p => p.Product)
                .WithMany(op => op.OrderProducts)
                .HasForeignKey(op => op.ProductId);

            modelBuilder.Entity<OrderProduct>()
                .HasOne(o => o.Order)
                .WithMany(op => op.OrderProducts)
                .HasForeignKey(op => op.OrderId);
        }
    }
}
