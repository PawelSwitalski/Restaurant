using AutoMapper;
using Restaurant.Business.Interfaces;
using Restaurant.Business.Models;
using Restaurant.Data.Interfaces;
using System.Collections.Concurrent;

namespace Restaurant.Business.Services
{
    public class StatisticService : IStatisticService
    {
        protected IMapper mapper { get; set; }
        protected IReceiptRepository receiptRepository { get; set; }
        protected IReceiptDetailRepository receiptDetailRepository { get; set; }

        public StatisticService(IMapper mapper, IReceiptRepository receiptRepository, IReceiptDetailRepository receiptDetailRepository)
        {
            this.mapper = mapper;
            this.receiptRepository = receiptRepository;
            this.receiptDetailRepository = receiptDetailRepository;
        }

        /// <inheritdoc cref="IStatisticService.GetCustomersMostPopularProductsAsync(int, int)"/>
        public async Task<IEnumerable<ProductModel>> GetCustomersMostPopularProductsAsync(int productCount, int customerId)
        {
            var receipts = await receiptRepository.GetAllWithDetailsAsync();

            var popularProducts = receipts
                .Where(r => r.CustomerId == customerId)
                .SelectMany(r => r.ReceiptDetails)
                .GroupBy(rd => rd.Product)
                .Select(g => new
                {
                    Product = g.Key,
                    TotalQuantity = g.Sum(rd => rd.Quantity)
                })
                .OrderByDescending(g => g.TotalQuantity)
                .Take(productCount)
                .ToList();


            ConcurrentBag<ProductModel> all = new ConcurrentBag<ProductModel>();

            Parallel.ForEach(popularProducts, (pp) =>
            {
                all.Add(mapper.Map<ProductModel>(pp.Product));
            });

            return all;
        }

        /// <inheritdoc cref="IStatisticService.GetIncomeOfCategoryInPeriod(int, DateTime, DateTime)"/>
        public async Task<decimal> GetIncomeOfCategoryInPeriod(int categoryId, DateTime startDate, DateTime endDate)
        {
            var receipts = await receiptRepository.GetAllWithDetailsAsync();

            var income = receipts
                .Where(r => r.OperationDate >= startDate && r.OperationDate <= endDate)
                .SelectMany(r => r.ReceiptDetails)
                .Where(rd => rd.Product.ProductCategoryId == categoryId)
                .Sum(rd => rd.DiscountUnitPrice * rd.Quantity);

            return income;
        }

        /// <inheritdoc cref="IStatisticService.GetMostPopularProductsAsync(int)"/>
        public async Task<IEnumerable<ProductModel>> GetMostPopularProductsAsync(int productCount)
        {
            var receiptDetails = await receiptDetailRepository.GetAllWithDetailsAsync();

            var mostPopulars = receiptDetails
                .GroupBy(rd => rd.ProductId)
                .Select(group => new
                {
                    ProductId = group.Key,
                    TotalQuantity = group.Sum(rd => rd.Quantity)
                })
                .OrderByDescending(result => result.TotalQuantity)
                .Take(productCount).ToList();

            ConcurrentBag<ProductModel> all = new ConcurrentBag<ProductModel>();

            Parallel.ForEach(mostPopulars, (mostPopular) =>
            {

                all.Add(mapper.Map<ProductModel>(
                    receiptDetails.First(d => d.ProductId == mostPopular.ProductId).Product
                    ));
            });

            return all;
        }
    }
}
