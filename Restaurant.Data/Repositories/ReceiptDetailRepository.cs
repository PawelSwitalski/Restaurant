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
    public class ReceiptDetailRepository : Repository<ReceiptDetail>, IReceiptDetailRepository
    {
        public ReceiptDetailRepository(RestaurantDbContext context) : base(context)
        {
        }

        public async Task<IEnumerable<ReceiptDetail>> GetAllWithDetailsAsync()
        {
            var query = context.Set<ReceiptDetail>()
                .Include(detail => detail.Receipt)
                .Include(detail => detail.Product)
                .ThenInclude(product => product.Category);

            return await query.ToListAsync();
        }

        public new async Task<ReceiptDetail> GetByIdAsync(int id)
        {
            return await context.ReceiptsDetails.Where(x => x.Id == id).FirstAsync();
        }

        public new async Task DeleteByIdAsync(int id)
        {
            ReceiptDetail entity = await context.ReceiptsDetails.Where(x => x.Id == id).FirstAsync();
            context.Remove(entity);
            await context.SaveChangesAsync();
        }
    }
}
