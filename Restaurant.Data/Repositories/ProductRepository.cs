using Restaurant.Data.Data;
using Restaurant.Data.Entities;
using Restaurant.Data.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Restaurant.Data.Repositories
{
    public class ProductRepository : Repository<Product>, IProductRepository
    {
        public ProductRepository(RestaurantDbContext context) : base(context)
        {
        }

        public new async Task AddAsync(Product entity)
        {
            var category = await context.Set<ProductCategory>().FirstOrDefaultAsync(x => x.Id == entity.ProductCategoryId);
            if (category != null && entity != null) 
            {
                entity.Category = category;
            }

            await context.AddAsync(entity);
            await context.SaveChangesAsync();
        }

        public async Task<IEnumerable<Product>> GetAllWithDetailsAsync()
        {
            var query = context.Set<Product>()
                .Include(product => product.Category)
                .Include(product => product.ReceiptDetails);

            return await query.ToListAsync();
        }

        public async Task<Product> GetByIdWithDetailsAsync(int id)
        {
            var product = await context.Set<Product>()
                .Include(product => product.Category)
                .Include(product => product.ReceiptDetails)
                .FirstOrDefaultAsync(c => c.Id == id);

            return product;
        }
    }
}
