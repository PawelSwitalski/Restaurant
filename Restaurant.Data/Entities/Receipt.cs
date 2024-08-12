using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Restaurant.Data.Entities
{
    public class Receipt : BaseEntity
    {
        [Required]
        public int CustomerId { get; set; }

        [Required]
        public DateTime OperationDate { get; set; } = DateTime.Now;

        public bool IsCheckedOut { get; set; } = false;

        public ICollection<ReceiptDetail> ReceiptDetails { get; set; }

        public Receipt() : base()
        {
        }

        public Receipt(int id, int customerId, DateTime operationDate, bool isCheckedOut) : base(id)
        {
            CustomerId = customerId;
            OperationDate = operationDate;
            IsCheckedOut = isCheckedOut;
        }
    }
}
