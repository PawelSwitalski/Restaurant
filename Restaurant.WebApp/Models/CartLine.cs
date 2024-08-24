using Restaurant.Business.Models;

namespace Restaurant.WebApp.Models
{
    public class CartLine
    {
        public int CartLineId { get; set; }

        public ProductModel Product { get; set; } = new();

        public int Quantity { get; set; }
    }
}
