using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using CartApp.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

namespace CartApp.Data
{
    public class ShopCartDbContext:IdentityDbContext
    {
        public ShopCartDbContext(DbContextOptions<ShopCartDbContext> options)
: base(options)
        { }

        public DbSet<Product> ProductSet { get; set; }
        public DbSet<IdentityUser> UserSet { get; set; }
        public DbSet<CartProduct> CartProductSet { get; set; }
        public DbSet<Order> OrdersSet { get; set; }
    }
}
