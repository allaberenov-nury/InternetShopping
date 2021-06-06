using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using InternetShopping.Models;

namespace InternetShopping.Data
{
    public class InternetShoppingDbContext : DbContext
    {
        public InternetShoppingDbContext(DbContextOptions<InternetShoppingDbContext> options)
            : base(options)
        {

        }

        public DbSet<Order> Orders { get; set; }
        public DbSet<Good> Goods { get; set; }
        public DbSet<OrderDetail> OrderDetails { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            /*
             modelBuilder
                 .Entity<Good>()
                 .HasMany<OrderDetail>(g => g.OrderDetails)
                 .WithOne(od => od.Good)
                 
            
            modelBuilder
                 .Entity<Order>()
                 .HasMany<OrderDetail>(g => g.OrderDetail)
                 .WithOne(od => od.Order)
                 .OnDelete(DeleteBehavior.Cascade);
            
            .*/
        }

    }
}
