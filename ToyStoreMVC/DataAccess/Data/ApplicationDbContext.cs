using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using ToyStoreMVC.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace ToyStoreMVC.DataAccess.Data
{
    public class ApplicationDbContext : IdentityDbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<Category> Categories { get; set; }

        public DbSet<Brand> Brands { get; set; } 

		public DbSet<Size> Sizes { get; set; }

        public DbSet<Product> Products { get; set; }

		public DbSet<ProductSize> ProductSizes { get; set; }

		public DbSet<ApplicationUser> ApplicationUsers { get; set; }

        public DbSet<ShoppingCart> ShoppingCarts { get; set; }
        public DbSet<OrderHeader> OrderHeaders { get; set; }
        public DbSet<OrderDetails> OrderDetails { get; set; }
    }
}
