using Microsoft.EntityFrameworkCore;
using Restaurant.Data.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Restaurant.Data.Data
{
    public class RestaurantDbContext : DbContext
    {

        public DbSet<Product> Products { get; set; }
        public DbSet<ProductCategory> ProductCategories { get; set; }
        public DbSet<Receipt> Receipts { get; set; }
        public DbSet<ReceiptDetail> ReceiptsDetails { get; set; }


        public RestaurantDbContext(DbContextOptions<RestaurantDbContext> options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            if (modelBuilder != null)
            {
                modelBuilder.Entity<ReceiptDetail>(entity =>
                {
                    entity.HasKey(e => new { e.ReceiptId, e.ProductId });
                    entity.HasAlternateKey(e => e.Id);
                });
            }

        }

    }
}
