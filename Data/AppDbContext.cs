using dotNET_Relationships.Models;
using Microsoft.EntityFrameworkCore;

namespace dotNET_Relationships.Data
{
    public class AppDbContext(DbContextOptions<AppDbContext> options): DbContext(options)
    {
        public DbSet<Product> Products { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<PaymentDetail> PaymentDetails { get; set; }
    }
}
