
using Clicktobuyservice.Models;
using Microsoft.EntityFrameworkCore;
using System;

namespace Clicktobuyservice
{
    public class ClicktoBuyDbContext : DbContext
    {
        public DbSet<ProductTbl> ProductTbl { get; set; }

        public DbSet<RegisterUserTbl> RegisterUserTbl { get; set; }

        public DbSet<ShoppingCart> ShoppingCart { get; set; }

        public DbSet<Order> Order{ get; set; }

        public DbSet<OrderAddress> OrderAddress { get; set; }

        public DbSet<UserAddress> UserAddress { get; set; }


        public ClicktoBuyDbContext(DbContextOptions<ClicktoBuyDbContext> options)
            : base(options)
        {
        }
    }
}
