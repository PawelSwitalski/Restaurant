using Restaurant.Business.Models;

namespace Restaurant.Business.Interfaces
{
    public interface IStatisticService
    {

        /// <summary>
        /// Asynchronously retrieves a collection of the most popular products overall, ordered by total quantity purchased across all receipts.
        /// </summary>
        /// <param name="productCount">The maximum number of products to return.</param>
        /// <returns>An asynchronous task that returns an IEnumerable of ProductModel objects representing the most popular products.</returns>
        Task<IEnumerable<ProductModel>> GetMostPopularProductsAsync(int productCount);

        /// <summary>
        /// Asynchronously retrieves a collection of the most popular products for a given customer, ordered by total quantity purchased.
        /// </summary>
        /// <param name="productCount">The maximum number of products to return.</param>
        /// <param name="customerId">The ID of the customer for whom to retrieve popular products.</param>
        /// <returns>An asynchronous task that returns an IEnumerable of ProductModel objects representing the customer's most popular products.</returns>
        Task<IEnumerable<ProductModel>> GetCustomersMostPopularProductsAsync(int productCount, int customerId);

        /// <summary>
        /// Asynchronously calculates the total income generated from a specific product category within a date period.
        /// </summary>
        /// <param name="categoryId">The ID of the product category to analyze.</param>
        /// <param name="startDate">The starting date of the period (inclusive).</param>
        /// <param name="endDate">The ending date of the period (inclusive).</param>
        /// <returns>An asynchronous task that returns a decimal value representing the total income for the category within the period.</returns>
        Task<decimal> GetIncomeOfCategoryInPeriod(int categoryId, DateTime startDate, DateTime endDate);
    }
}
