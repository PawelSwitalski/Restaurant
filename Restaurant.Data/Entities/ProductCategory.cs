using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Restaurant.Data.Entities
{
    public class ProductCategory : BaseEntity
    {
        [Required]
        [MaxLength(70)]
        public string CategoryName { get; set; }

        public ICollection<Product> Products { get; set; }

        public ProductCategory() : base()
        {
        }
        public ProductCategory(int id, string categoryName) : base(id)
        {
            CategoryName = categoryName;
        }
    }
}
