using Microsoft.EntityFrameworkCore;
using Foodie.Models;

namespace Foodie.Models
{
    public class FoodieDBContext: DbContext
    {
        public FoodieDBContext(DbContextOptions options) : base(options)
        {
            
        }
        public DbSet<Product> Products { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<Customer> Customers { get; set; }
        public DbSet<Cart> Carts { get; set; }

        public DbSet<Order> Orders { get; set; }

    }
}
