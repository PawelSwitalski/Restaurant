using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Restaurant.Data.Entities
{
    public class ReceiptDetail : BaseEntity
    {
        [Required]
        [ForeignKey(nameof(Receipt))]
        public int ReceiptId { get; set; }

        [Required]
        [ForeignKey(nameof(Product))]
        public int ProductId { get; set; }

        [Column(TypeName = "decimal(5, 2)")]
        public decimal DiscountUnitPrice { get; set; }

        [Column(TypeName = "decimal(5, 2)")]
        public decimal UnitPrice { get; set; }

        public int Quantity { get; set; }

        public Receipt Receipt { get; set; }

        public Product Product { get; set; }


        public ReceiptDetail() : base()
        {
        }

        public ReceiptDetail(int id, int receiptId, int productId, decimal discountUnitPrice, decimal unitPrice, int quantity) : base(id)
        {
            ReceiptId = receiptId;
            ProductId = productId;
            DiscountUnitPrice = discountUnitPrice;
            UnitPrice = unitPrice;
            Quantity = quantity;
        }
    }
}
