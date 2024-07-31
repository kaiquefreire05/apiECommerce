using ECommerceApi.Database.Maps;
using ECommerceApi.Models;
using Microsoft.EntityFrameworkCore;

namespace ECommerceApi.Database
{
    public class ECommerceDBContext : DbContext
    {
        public ECommerceDBContext(DbContextOptions options): base(options)
        {

        }

        // Representation of tables
        public DbSet<OrderItemModel> OrderItems { get; set; }
        public DbSet<OrderModel> Orders { get; set; }
        public DbSet<ProductsModel> Products { get; set; }
        public DbSet<UsersModel> User { get; set; }

        protected override void OnModelCreating (ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new OrderMap());
            modelBuilder.ApplyConfiguration(new UserMap());
            modelBuilder.ApplyConfiguration(new ProductsMap());
            modelBuilder.ApplyConfiguration(new OrderItemMap());
            base.OnModelCreating(modelBuilder);
        }
    }
}
