using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Restaurant.Data.Entities
{
    public class Product : BaseEntity
    {
        [Required]
        [ForeignKey(nameof(Product))]
        public int ProductCategoryId { get; set; }

        [Required]
        [Column(TypeName = "nvarchar(100)")]
        public string ProductName { get; set; }

        [Column(TypeName = "nvarchar(2000)")]
        public string ProductDescription { get; set; }

        [Required]
        [Column(TypeName = "decimal(5, 2)")]
        public decimal Price { get; set; }

        [Column(TypeName = "varchar(200)")]
        public string ImagePath { get; set; }

        public ProductCategory Category { get; set; }

        public ICollection<ReceiptDetail> ReceiptDetails { get; set; }

        public Product() : base()
        {
        }

        public Product(int id, int productCategoryId, string productName, decimal price, string imagePath) : base(id)
        {
            ProductCategoryId = productCategoryId;
            ProductName = productName;
            Price = price;
            ImagePath = imagePath;
        }
    }
}
