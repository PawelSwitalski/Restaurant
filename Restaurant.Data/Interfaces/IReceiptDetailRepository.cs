using Restaurant.Data.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Restaurant.Data.Interfaces
{
    public interface IReceiptDetailRepository : IRepository<ReceiptDetail>
    {
        public Task<IEnumerable<ReceiptDetail>> GetAllWithDetailsAsync();

    }
}
