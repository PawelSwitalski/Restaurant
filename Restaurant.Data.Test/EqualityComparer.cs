using System.Diagnostics.CodeAnalysis;
using Restaurant.Data.Entities;

namespace Restaurant.Data.Test
{
    public class ReceiptEqualityComparer : IEqualityComparer<Receipt>
    {
        public bool Equals([AllowNull] Receipt x, [AllowNull] Receipt y)
        {
            if (x == null && y == null)
                return true;
            if (x == null || y == null)
                return false;

            return x.Id == y.Id
                && x.CustomerId == y.CustomerId
                && x.OperationDate == y.OperationDate
                && x.IsCheckedOut == y.IsCheckedOut;
        }

        public int GetHashCode([DisallowNull] Receipt obj)
        {
            return obj.GetHashCode();
        }
    }

    internal class ProductEqualityComparer : IEqualityComparer<Product>
    {
        public bool Equals([AllowNull] Product x, [AllowNull] Product y)
        {
            if (x == null && y == null)
                return true;
            if (x == null || y == null)
                return false;

            return x.Id == y.Id
                && x.ProductCategoryId == y.ProductCategoryId
                && x.ProductName == y.ProductName
                && x.Price == y.Price;
        }

        public int GetHashCode([DisallowNull] Product obj)
        {
            return obj.GetHashCode();
        }
    }

    internal class ProductCategoryEqualityComparer : IEqualityComparer<ProductCategory>
    {
        public bool Equals([AllowNull] ProductCategory x, [AllowNull] ProductCategory y)
        {
            if (x == null && y == null)
                return true;
            if (x == null || y == null)
                return false;

            return x.Id == y.Id
                && x.CategoryName == y.CategoryName;
        }

        public int GetHashCode([DisallowNull] ProductCategory obj)
        {
            return obj.GetHashCode();
        }
    }

    internal class ReceiptDetailEqualityComparer : IEqualityComparer<ReceiptDetail>
    {
        public bool Equals([AllowNull] ReceiptDetail x, [AllowNull] ReceiptDetail y)
        {
            if (x == null && y == null)
                return true;
            if (x == null || y == null)
                return false;

            return x.Id == y.Id
                && x.ReceiptId == y.ReceiptId
                && x.ProductId == y.ProductId
                && x.UnitPrice == y.UnitPrice
                && x.DiscountUnitPrice == y.DiscountUnitPrice
                && x.Quantity == y.Quantity;
        }

        public int GetHashCode([DisallowNull] ReceiptDetail obj)
        {
            return obj.GetHashCode();
        }
    }

}
