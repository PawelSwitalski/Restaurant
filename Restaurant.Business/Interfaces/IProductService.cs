using Restaurant.Business.Models;

namespace Restaurant.Business.Interfaces
{
    public interface IProductService : ICrud<ProductModel>
    {
        /// <summary>
        /// Asynchronously retrieves a collection of product models filtered based on the provided search criteria.
        /// </summary>
        /// <param name="filterSearch">The search criteria used to filter the products.</param>
        /// <exception cref="ArgumentNullException">Thrown when the provided filter search criteria is null.</exception>
        /// <returns>An asynchronous task that, upon completion, returns an IEnumerable of ProductModel objects representing the filtered products.</returns>
        Task<IEnumerable<ProductModel>> GetByFilterAsync(FilterSearchModel filterSearch);

        /// <summary>
        /// Asynchronously retrieves a collection of product category models representing all product categories in the system.
        /// </summary>
        /// <returns>An asynchronous task that, upon completion, returns an IEnumerable of ProductCategoryModel objects representing all product categories.</returns>
        Task<IEnumerable<ProductCategoryModel>> GetAllProductCategoriesAsync();

        /// <summary>
        /// Asynchronously adds a new product category to the system based on the provided product category model.
        /// </summary>
        /// <param name="categoryModel">The product category model containing the information for the new category.</param>
        /// <exception cref="MarketException">Thrown when the category name in the model is empty or model is null.</exception>
        /// <returns>task</returns>
        Task AddCategoryAsync(ProductCategoryModel categoryModel);

        /// <summary>
        /// Asynchronously updates a product category in the system based on the provided product category model.
        /// </summary>
        /// <param name="categoryModel">The product category model containing the updated information.</param>
        /// <exception cref="ArgumentNullException">Thrown when the provided category model is null.</exception>
        /// <exception cref="MarketException">Thrown when the category name in the model is empty.</exception>
        /// <returns>An asynchronous task representing the update operation.</returns>
        Task UpdateCategoryAsync(ProductCategoryModel categoryModel);

        /// <summary>
        /// Asynchronously removes a product category from the system based on its identifier.
        /// </summary>
        /// <param name="categoryId">The identifier of the product category to remove.</param>
        /// <returns>An asynchronous task representing the removal operation.</returns>
        Task RemoveCategoryAsync(int categoryId);
    }
}
