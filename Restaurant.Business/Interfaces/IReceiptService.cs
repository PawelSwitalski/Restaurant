using Restaurant.Business.Models;

namespace Restaurant.Business.Interfaces
{
    public interface IReceiptService : ICrud<ReceiptModel>
    {

        /// <summary>
        /// Asynchronously adds a product to a receipt.
        /// </summary>
        /// <param name="productId">The ID of the product to add.</param>
        /// <param name="receiptId">The ID of the receipt to which to add the product.</param>
        /// <param name="quantity">The quantity of the product to add.</param>
        /// <exception cref="MarketException">Thrown if the receipt with the provided ID is not found.</exception>
        /// <returns>An asynchronous task.</returns>
        Task AddProductAsync(int productId, int receiptId, int quantity);

        /// <summary>
        /// Asynchronously removes a specified quantity of a product from a receipt.
        /// </summary>
        /// <param name="productId">The ID of the product to remove.</param>
        /// <param name="receiptId">The ID of the receipt from which to remove the product.</param>
        /// <param name="quantity">The quantity of the product to remove.</param>
        /// <returns>An asynchronous task.</returns>
        Task RemoveProductAsync(int productId, int receiptId, int quantity);

        /// <summary>
        /// Asynchronously retrieves a collection of receipt details for a specified receipt.
        /// </summary>
        /// <param name="receiptId">The ID of the receipt for which to retrieve details.</param>
        /// <returns>An asynchronous task that returns an IEnumerable of ReceiptDetailModel objects representing the receipt's details.</returns>
        Task<IEnumerable<ReceiptDetailModel>> GetReceiptDetailsAsync(int receiptId);

        /// <summary>
        /// Asynchronously calculates the total amount to be paid for a receipt based on its identifier.
        /// </summary>
        /// <param name="receiptId">The identifier of the receipt to calculate the total for.</param>
        /// <returns>An asynchronous task that, upon completion, returns a decimal value representing the total amount to be paid for the receipt.</returns>
        Task<decimal> ToPayAsync(int receiptId);

        /// <summary>
        /// Asynchronously marks a receipt as checked out.
        /// </summary>
        /// <param name="receiptId">The ID of the receipt to check out.</param>
        /// <returns>An asynchronous task.</returns>
        Task CheckOutAsync(int receiptId);

        /// <summary>
        /// Asynchronously retrieves a collection of receipts within a specified date period.
        /// </summary>
        /// <param name="startDate">The starting date of the period.</param>
        /// <param name="endDate">The ending date of the period.</param>
        /// <returns>An asynchronous task that returns an IEnumerable of ReceiptModel objects representing the receipts within the period.</returns>
        Task<IEnumerable<ReceiptModel>> GetReceiptsByPeriodAsync(DateTime startDate, DateTime endDate);
    }
}
