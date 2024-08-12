using Restaurant.Data.Data;
using Restaurant.Data.Entities;
using Restaurant.Data.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Restaurant.Data.Repositories
{
    public class ProductCategoryRepository : Repository<ProductCategory>, IProductCategoryRepository
    {
        public ProductCategoryRepository(RestaurantDbContext context) : base(context)
        {
        }
    }
}
