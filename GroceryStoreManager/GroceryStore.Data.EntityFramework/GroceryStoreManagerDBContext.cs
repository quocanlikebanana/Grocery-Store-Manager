using GroceryStore.Domain.Model;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;

namespace GroceryStore.Data.EntityFramework
{
    public class GroceryStoreManagerDBContext : DbContext
    {
        public DbSet<Coupon> Coupon
        {
            get; set;
        }
        public DbSet<Customer> Customer
        {
            get; set;
        }
        public DbSet<Order> Order
        {
            get; set;
        }
        public DbSet<OrderDetail> OrderDetail
        {
            get; set;
        }
        public DbSet<Product> Product
        {
            get; set;
        }
        public DbSet<ProductType> ProductType
        {
            get; set;
        }

        public string connectionString
        {
            get; set;
        }

        public GroceryStoreManagerDBContext(string connectionString)
        {
            this.connectionString = connectionString;
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<OrderDetail>().HasNoKey();
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(connectionString);
            base.OnConfiguring(optionsBuilder);
        }
    }
}
