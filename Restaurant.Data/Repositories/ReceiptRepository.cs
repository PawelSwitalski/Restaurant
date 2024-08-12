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
    public class ReceiptRepository : Repository<Receipt>, IReceiptRepository
    {
        public ReceiptRepository(RestaurantDbContext context) : base(context)
        {
        }

        public async Task<IEnumerable<Receipt>> GetAllWithDetailsAsync()
        {
            var query = context.Set<Receipt>()
                .Include(product => product.ReceiptDetails)
                .ThenInclude(detail => detail.Product)
                .ThenInclude(product => product.Category);

            return await query.ToListAsync();
        }

        public async Task<Receipt> GetByIdWithDetailsAsync(int id)
        {
            var product = await context.Set<Receipt>()
                .Include(product => product.ReceiptDetails)
                .ThenInclude(detail => detail.Product)
                .ThenInclude(product => product.Category)
                .FirstOrDefaultAsync(c => c.Id == id);

            return product;
        }
    }
}
