using AutoMapper;
using Restaurant.Business.Interfaces;
using Restaurant.Business.Models;
using Restaurant.Business.Validation;
using Restaurant.Data.Entities;
using Restaurant.Data.Interfaces;
using System.Collections.Concurrent;

namespace Restaurant.Business.Services
{
    public class ReceiptService : IReceiptService
    {
        protected IMapper mapper { get; set; }
        protected IReceiptRepository receiptRepository { get; set; }
        protected IProductRepository productRepository { get; set; }
        protected IReceiptDetailRepository receiptDetailRepository { get; set; }

        public ReceiptService(IReceiptRepository receiptRepository, IProductRepository productRepository,  IMapper mapper, IReceiptDetailRepository receiptDetailRepository)
        {
            this.receiptRepository = receiptRepository;
            this.productRepository = productRepository;
            this.mapper = mapper;
            this.receiptDetailRepository = receiptDetailRepository;
        }

        public async Task AddAsync(ReceiptModel model)
        {
            var receipt = mapper.Map<Receipt>(model);
            await receiptRepository.AddAsync(receipt);
        }

        /// <inheritdoc cref="IReceiptService.AddProductAsync(int, int, int)"/>
        public async Task AddProductAsync(int productId, int receiptId, int quantity)
        {
            Receipt receipt = await receiptRepository.GetByIdWithDetailsAsync(receiptId);

            if (receipt == null)
            {
                throw new RestaurantException();
            }

            ReceiptDetail detail = null;

            try
            {
                detail = receipt.ReceiptDetails.FirstOrDefault(d => d.ProductId == productId);
            }
            catch (ArgumentNullException)
            {
                // Handle exception or log error message
            }

            if (detail != null)
            {
                detail.Quantity += quantity;
            }
            else
            {
                ReceiptDetail receiptDetail = new ReceiptDetail
                {
                    ProductId = productId,
                    ReceiptId = receiptId,
                    Quantity = quantity
                };

                try
                {
                    Product product = null;
                    product = await productRepository.GetByIdAsync(productId);

                    receiptDetail.UnitPrice = product.Price;
                    // TODO: Change this (DiscountUnitPrice)
                    receiptDetail.DiscountUnitPrice = product.Price;
                }
                catch (NullReferenceException ex)
                {
                    throw new RestaurantException("NullReferenceException product is Null. ", ex);
                }

                await receiptDetailRepository.AddAsync(receiptDetail);
            }
        }


        /// <inheritdoc cref="IReceiptService.CheckOutAsync(int)"/>
        public async Task CheckOutAsync(int receiptId)
        {
            Receipt receipt = await receiptRepository.GetByIdAsync(receiptId);
            if (receipt != null) 
            { 
                receipt.IsCheckedOut = true;
                receiptRepository.Update(receipt);
            }
        }

        public async Task DeleteAsync(int modelId)
        {
            Receipt receipt = await receiptRepository.GetByIdWithDetailsAsync(modelId);

            foreach (var detail in receipt.ReceiptDetails)
            {
                receiptDetailRepository.Delete(detail);
            }

            await receiptRepository.DeleteByIdAsync(modelId);
        }

        public async Task<IEnumerable<ReceiptModel>> GetAllAsync()
        {
            var receipts = await receiptRepository.GetAllWithDetailsAsync();

            ConcurrentBag<ReceiptModel> all = new ConcurrentBag<ReceiptModel>();

            Parallel.ForEach(receipts, (receipt) =>
            {
                all.Add(mapper.Map<ReceiptModel>(receipt));
            });

            return all;
        }

        public async Task<ReceiptModel> GetByIdAsync(int id)
        {
            Receipt product = await receiptRepository.GetByIdWithDetailsAsync(id);

            return mapper.Map<ReceiptModel>(product);
        }

        /// <inheritdoc cref="IReceiptService.GetReceiptDetailsAsync(int)"/>
        public async Task<IEnumerable<ReceiptDetailModel>> GetReceiptDetailsAsync(int receiptId)
        {
            var receipt = await receiptRepository.GetByIdWithDetailsAsync(receiptId);

            var details = receipt.ReceiptDetails;

            ConcurrentBag<ReceiptDetailModel> models = new ConcurrentBag<ReceiptDetailModel>();
            Parallel.ForEach(details, d =>
            {
                models.Add(mapper.Map<ReceiptDetailModel>(d));
            });

            return models;
        }

        /// <inheritdoc cref="IReceiptService.GetReceiptsByPeriodAsync(DateTime, DateTime)"/>
        public async Task<IEnumerable<ReceiptModel>> GetReceiptsByPeriodAsync(DateTime startDate, DateTime endDate)
        {
            var receipts = await receiptRepository.GetAllWithDetailsAsync();

            var filtered = receipts.Where(r => r.OperationDate > startDate && r.OperationDate < endDate);

            ConcurrentBag<ReceiptModel> models = new ConcurrentBag<ReceiptModel>();
            Parallel.ForEach(filtered, f => 
            {
                models.Add(mapper.Map<ReceiptModel>(f));
            });

            return models;
        }

        /// <inheritdoc cref="IReceiptService.RemoveProductAsync(int, int, int)"/>
        public async Task RemoveProductAsync(int productId, int receiptId, int quantity)
        {
            Receipt receipt = await receiptRepository.GetByIdWithDetailsAsync(receiptId);

            var detail = receipt.ReceiptDetails.First(d => d.ProductId == productId);

            detail.Quantity -= quantity;

            if (detail.Quantity <= 0) 
            {
                receiptDetailRepository.Delete(detail);
            }
        }

        /// <inheritdoc cref="IReceiptService.ToPayAsync(int)"/>
        public async Task<decimal> ToPayAsync(int receiptId)
        {
            var receipt = await receiptRepository.GetByIdWithDetailsAsync(receiptId);

            decimal sum = 0;

            foreach (var detail in receipt.ReceiptDetails)
            {
                sum += detail.DiscountUnitPrice * detail.Quantity;
            }

            return sum;
        }

        public async Task UpdateAsync(ReceiptModel model)
        {
            var entitie = mapper.Map<Receipt>(model);
            receiptRepository.Update(entitie);
        }
    }
}
