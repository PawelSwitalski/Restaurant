using Restaurant.Business.Models;
using Restaurant.Data.Entities;

namespace Restaurant.WebApp.Models
{
    public class Cart
    {
        private readonly List<CartLine> lines = new List<CartLine>();

        public IReadOnlyList<CartLine> Lines
        {
            get { return this.lines; }
        }

        public virtual void AddItem(ProductModel product, int quantity)
        {
            CartLine? line;

            try
            {
                line = this.lines.First(p => p.Product.Id == product.Id);
            }
            catch (ArgumentNullException)
            {
                line = null;
            }

            if (line is null)
            {
                this.lines.Add(new CartLine
                {
                    Product = product,
                    Quantity = quantity,
                });
            }
            else
            {
                line.Quantity += quantity;
            }
        }

        public virtual void RemoveLine(ProductModel product)
            => this.lines.RemoveAll(l => l.Product.Id == product.Id);

        public decimal ComputeTotalValue()
            => this.lines.Sum(e => e.Product.Price * e.Quantity);

        public virtual void Clear() => this.lines.Clear();
    }
}
