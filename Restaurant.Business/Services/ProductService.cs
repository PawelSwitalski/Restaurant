using AutoMapper;
using Restaurant.Business.Interfaces;
using Restaurant.Business.Models;
using Restaurant.Business.Validation;
using System.Collections.Concurrent;
using Restaurant.Data.Entities;
using Restaurant.Data.Interfaces;

namespace Restaurant.Business.Services
{
    public class ProductService : IProductService
    {
        protected IProductRepository productRepository { get; set; }
        protected IProductCategoryRepository productCategoryRepository { get; set; }
        protected IMapper mapper { get; set; }

        public ProductService(IProductRepository productRepository,
                              IProductCategoryRepository productCategoryRepository,
                              IMapper mapper)
        {
            this.productRepository = productRepository;
            this.productCategoryRepository = productCategoryRepository;
            this.mapper = mapper;
        }

        public async Task AddAsync(ProductModel model)
        {
            if (model == null)
                throw new RestaurantException();

            if (string.IsNullOrEmpty(model.ProductName))
                throw new RestaurantException("Product data missing.");

            if (model.Price <= 0)
                throw new RestaurantException("Product price is zero or lower.");

            var product = mapper.Map<Product>(model);
            await this.productRepository.AddAsync(product);
        }

        /// <inheritdoc cref="IProductService.AddCategoryAsync(ProductCategoryModel)"/>
        public async Task AddCategoryAsync(ProductCategoryModel categoryModel)
        {
            if (categoryModel == null) 
                throw new RestaurantException();

            if (string.IsNullOrEmpty(categoryModel.CategoryName))
                throw new RestaurantException("CategoryName is missing.");

            var category = mapper.Map<ProductCategory>(categoryModel);
            await productCategoryRepository.AddAsync(category);
        }

        public async Task DeleteAsync(int modelId)
        {
            await productRepository.DeleteByIdAsync(modelId);
        }

        public async Task<IEnumerable<ProductModel>> GetAllAsync()
        {
            var products = await productRepository.GetAllWithDetailsAsync();

            ConcurrentBag<ProductModel> models = new ConcurrentBag<ProductModel>();
            Parallel.ForEach(products, p =>
            {
                models.Add(mapper.Map<ProductModel>(p));
            });

            return models;
        }

        /// <inheritdoc cref="IProductService.GetAllProductCategoriesAsync"/>
        public async Task<IEnumerable<ProductCategoryModel>> GetAllProductCategoriesAsync()
        {
            var pCategories = await productCategoryRepository.GetAllAsync();

            ConcurrentBag<ProductCategoryModel> models = new ConcurrentBag<ProductCategoryModel>();
            Parallel.ForEach(pCategories, pc =>
            {
                models.Add(mapper.Map<ProductCategoryModel>(pc));
            });

            return models;
        }

        /// <inheritdoc cref="IProductService.GetByFilterAsync(FilterSearchModel)"/>
        public async Task<IEnumerable<ProductModel>> GetByFilterAsync(FilterSearchModel filterSearch)
        {
            if (filterSearch == null)
                throw new ArgumentNullException(nameof(filterSearch));

            //var products = await unitOfWork.ProductRepository.GetAllWithDetailsAsync();
            var products = await productRepository.GetAllWithDetailsAsync();

            IEnumerable<Product> filtered = products;

            if (filterSearch.CategoryId != null)
                filtered = filtered.Where(x => x.ProductCategoryId == filterSearch.CategoryId);

            if (filterSearch.MaxPrice != null)
                filtered = filtered.Where(x => x.Price < filterSearch.MaxPrice);

            if (filterSearch.MinPrice != null)
                filtered = filtered.Where (x => x.Price > filterSearch.MinPrice);


            ConcurrentBag<ProductModel> models = new ConcurrentBag<ProductModel>();
            Parallel.ForEach(filtered, f =>
            {
                models.Add(mapper.Map<ProductModel>(f));
            });

            return models;
        }

        public async Task<ProductModel> GetByIdAsync(int id)
        {
            Product product = await productRepository.GetByIdWithDetailsAsync(id);

            return mapper.Map<ProductModel>(product);
        }

        /// <inheritdoc cref="IProductService.RemoveCategoryAsync(int)"/>
        public async Task RemoveCategoryAsync(int categoryId)
        {
            await productCategoryRepository.DeleteByIdAsync(categoryId);
        }

        /// <summary>
        /// Asynchronously updates a product in the system based on the provided product model.
        /// </summary>
        /// <param name="model">The product model containing the updated information.</param>
        /// <exception cref="ArgumentNullException">Thrown when the provided product model is null.</exception>
        /// <exception cref="MarketException">Thrown when the product name in the model is empty.</exception>
        /// <returns>An asynchronous task representing the update operation.</returns>
        public async Task UpdateAsync(ProductModel model)
        {
            if (model == null)
                throw new ArgumentNullException(nameof(model));

            if (string.IsNullOrEmpty(model.ProductName))
                throw new RestaurantException("Product name is empty");

            var pUpdate = mapper.Map<Product>(model);
            productRepository.Update(pUpdate);
        }

        /// <inheritdoc cref="IProductService.UpdateCategoryAsync(ProductCategoryModel)"/>
        public async Task UpdateCategoryAsync(ProductCategoryModel categoryModel)
        {
            if (categoryModel == null)
                throw new ArgumentNullException(nameof(categoryModel));

            if (string.IsNullOrEmpty(categoryModel.CategoryName))
                throw new RestaurantException("Category Name is empty");

            var cUpdate = mapper.Map<ProductCategory>(categoryModel);
            productCategoryRepository.Update(cUpdate);
        }
    }
}
