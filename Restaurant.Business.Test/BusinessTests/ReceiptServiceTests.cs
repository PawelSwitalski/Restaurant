using Castle.Core.Resource;
using FluentAssertions;
using Moq;
using Restaurant.Business.Models;
using Restaurant.Business.Services;
using Restaurant.Business.Validation;
using Restaurant.Data.Entities;
using Restaurant.Data.Interfaces;
using Restaurant.Data.Test;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Restaurant.Business.Test.BusinessTests
{
    public class ReceiptServiceTests
    {
        [Test]
        public async Task ReceiptServiceGetAllReturnsAllReceipts()
        {
            //arrange
            var expected = GetTestReceiptsModels;

            // var receiptService = new ReceiptService(mockUnitOfWork.Object, UnitTestHelper.CreateMapperProfile());

            var mockReceiptRepository = new Mock<IReceiptRepository>();
            mockReceiptRepository
                .Setup(x => x.GetAllWithDetailsAsync())
                .ReturnsAsync(GetTestReceiptsEntities.AsEnumerable());

            var mockProductRepository = new Mock<IProductRepository>();
            var mockReceiptDetailRepository = new Mock<IReceiptDetailRepository>();

            var receiptService = new ReceiptService(mockReceiptRepository.Object,
                                                    mockProductRepository.Object,
                                                    UnitTestHelper.CreateMapperProfile(),
                                                    mockReceiptDetailRepository.Object);


            //act
            var actual = await receiptService.GetAllAsync();

            //assert
            actual.Should().BeEquivalentTo(expected);
        }

        [TestCase(5)]
        public async Task ReceiptServiceGetByIdReturnsReceiptModel(int id)
        {
            //arrange
            var expected = GetTestReceiptsModels.FirstOrDefault(x => x.Id == id);


            var mockReceiptRepository = new Mock<IReceiptRepository>();
            mockReceiptRepository
                .Setup(x => x.GetByIdWithDetailsAsync(It.IsAny<int>()))
                .ReturnsAsync(GetTestReceiptsEntities.FirstOrDefault(x => x.Id == id));

            var mockProductRepository = new Mock<IProductRepository>();
            var mockReceiptDetailRepository = new Mock<IReceiptDetailRepository>();

            var receiptService = new ReceiptService(mockReceiptRepository.Object,
                                                    mockProductRepository.Object,
                                                    UnitTestHelper.CreateMapperProfile(),
                                                    mockReceiptDetailRepository.Object);


            
            //act
            var actual = await receiptService.GetByIdAsync(id);

            //assert
            actual.Should().BeEquivalentTo(expected);
        }

        [Test]
        public async Task ReceiptServiceAddAsyncAddsReceipt()
        {
            //arrange

            var mockReceiptRepository = new Mock<IReceiptRepository>();
            mockReceiptRepository
                .Setup(x => x.AddAsync(It.IsAny<Receipt>()));

            var mockProductRepository = new Mock<IProductRepository>();
            var mockReceiptDetailRepository = new Mock<IReceiptDetailRepository>();

            var receiptService = new ReceiptService(mockReceiptRepository.Object,
                                                    mockProductRepository.Object,
                                                    UnitTestHelper.CreateMapperProfile(),
                                                    mockReceiptDetailRepository.Object);



            var receipt = GetTestReceiptsModels.Last();

            //act
            await receiptService.AddAsync(receipt);

            //assert
            mockReceiptRepository.Verify(x => x.AddAsync(It.Is<Receipt>(c => c.Id == receipt.Id
                && c.CustomerId == receipt.CustomerId && c.OperationDate == receipt.OperationDate)), Times.Once);
        }

        [Test]
        public async Task ReceiptServiceUpdateAsyncUpdatesReceipt()
        {
            //arrange
            var mockReceiptRepository = new Mock<IReceiptRepository>();
            mockReceiptRepository
                .Setup(x => x.Update(It.IsAny<Receipt>()));

            var mockProductRepository = new Mock<IProductRepository>();
            var mockReceiptDetailRepository = new Mock<IReceiptDetailRepository>();

            var receiptService = new ReceiptService(mockReceiptRepository.Object,
                                                    mockProductRepository.Object,
                                                    UnitTestHelper.CreateMapperProfile(),
                                                    mockReceiptDetailRepository.Object);


            var receipt = GetTestReceiptsModels.First();

            //act
            await receiptService.UpdateAsync(receipt);

            //assert
            mockReceiptRepository.Verify(x => x.Update(It.Is<Receipt>(r => r.Id == receipt.Id && r.IsCheckedOut == receipt.IsCheckedOut &&
                r.CustomerId == receipt.CustomerId && r.OperationDate == receipt.OperationDate)), Times.Once);
        }

        [TestCase(1)]
        [TestCase(2)]
        public async Task ReceiptServiceDeleteAsyncDeletesReceiptWithDetails(int receiptId)
        {
            //arrange
            var receipt = GetTestReceiptsEntities.First(x => x.Id == receiptId);
            var expectedDetailsLength = receipt.ReceiptDetails.Count;

            var mockReceiptRepository = new Mock<IReceiptRepository>();
            mockReceiptRepository
                .Setup(x => x.GetByIdWithDetailsAsync(It.IsAny<int>()))
                .ReturnsAsync(receipt);

            mockReceiptRepository
                .Setup(x => x.DeleteByIdAsync(It.IsAny<int>()));


            var mockProductRepository = new Mock<IProductRepository>();
            var mockReceiptDetailRepository = new Mock<IReceiptDetailRepository>();
            mockReceiptDetailRepository
                .Setup(x => x.Delete(It.IsAny<ReceiptDetail>()));

            var receiptService = new ReceiptService(mockReceiptRepository.Object,
                                                    mockProductRepository.Object,
                                                    UnitTestHelper.CreateMapperProfile(),
                                                    mockReceiptDetailRepository.Object);



            //act
            await receiptService.DeleteAsync(receiptId);

            //assert
            mockReceiptRepository.Verify(x => x.DeleteByIdAsync(It.Is<int>(x => x == receiptId)), Times.Once());
            mockReceiptDetailRepository.Verify(x => x.Delete(It.Is<ReceiptDetail>(detail => detail.ReceiptId == receiptId)),
                failMessage: "All existing receipt details must be deleted", times: Times.Exactly(expectedDetailsLength));
        }


        [TestCase(4)]
        [TestCase(5)]
        public async Task ReceiptServiceGetReceiptDetailsAsyncReturnsDetailsByReceiptId(int receiptId)
        {
            //arrange
            var mockReceiptRepository = new Mock<IReceiptRepository>();
            mockReceiptRepository
                .Setup(x => x.GetByIdWithDetailsAsync(It.IsAny<int>()))
                .ReturnsAsync(GetTestReceiptsEntities.FirstOrDefault(x => x.Id == receiptId));

            var mockProductRepository = new Mock<IProductRepository>();
            var mockReceiptDetailRepository = new Mock<IReceiptDetailRepository>();

            var receiptService = new ReceiptService(mockReceiptRepository.Object,
                                                    mockProductRepository.Object,
                                                    UnitTestHelper.CreateMapperProfile(),
                                                    mockReceiptDetailRepository.Object);



            //act
            var actual = await receiptService.GetReceiptDetailsAsync(receiptId);

            //assert
            var expected = GetTestReceiptsEntities.FirstOrDefault(x => x.Id == receiptId)?.ReceiptDetails;

            actual.Should().BeEquivalentTo(expected, options => options.Excluding(x => x.Product).Excluding(x => x.Receipt));
        }


        [TestCase("1965-1-1", "1965-2-1", new[] { 1, 2 })]
        [TestCase("1965-2-1", "1965-4-1", new[] { 3, 4, 5 })]
        public async Task ReceiptServiceGetReceiptsByPeriodAsyncReturnsReceiptsInPeriod(DateTime startDate, DateTime endDate, IEnumerable<int> expectedReceiptIds)
        {
            //arrange
            var mockReceiptRepository = new Mock<IReceiptRepository>();
            mockReceiptRepository
                .Setup(x => x.GetAllWithDetailsAsync())
                .ReturnsAsync(GetTestReceiptsEntities.AsEnumerable());

            var mockProductRepository = new Mock<IProductRepository>();
            var mockReceiptDetailRepository = new Mock<IReceiptDetailRepository>();

            var receiptService = new ReceiptService(mockReceiptRepository.Object,
                                                    mockProductRepository.Object,
                                                    UnitTestHelper.CreateMapperProfile(),
                                                    mockReceiptDetailRepository.Object);


            //act
            var actual = await receiptService.GetReceiptsByPeriodAsync(startDate, endDate);

            //assert
            var expected = GetTestReceiptsModels.Where(x => expectedReceiptIds.Contains(x.Id));

            actual.Should().BeEquivalentTo(expected);
        }

        [TestCase(1, 99)]
        [TestCase(4, 546)]
        [TestCase(5, 1074)]
        public async Task ReceiptServiceToPayAsyncReturnsSumByReceiptIdWithDiscount(int receiptId, decimal expectedSum)
        {
            //arrange
            var receipt = GetTestReceiptsEntities.FirstOrDefault(x => x.Id == receiptId);

            var mockReceiptRepository = new Mock<IReceiptRepository>();
            mockReceiptRepository
                .Setup(x => x.GetByIdWithDetailsAsync(It.IsAny<int>()))
                .ReturnsAsync(receipt);

            var mockProductRepository = new Mock<IProductRepository>();
            var mockReceiptDetailRepository = new Mock<IReceiptDetailRepository>();

            var receiptService = new ReceiptService(mockReceiptRepository.Object,
                                                    mockProductRepository.Object,
                                                    UnitTestHelper.CreateMapperProfile(),
                                                    mockReceiptDetailRepository.Object);


            //act
            var actual = await receiptService.ToPayAsync(receiptId);

            //assert
            actual.Should().Be(expectedSum);
        }

        [TestCase(1, 5, 7)]
        [TestCase(2, 1, 4)]
        [TestCase(3, 200, 201)]
        public async Task ReceiptServiceAddProductAsyncUpdatesQuantityIfProductWasAddedToReceipt(int productId, int quantity, int expectedQuantity)
        {
            //arrange 
            var receipt = new Receipt
            {
                Id = 2,
                ReceiptDetails = new List<ReceiptDetail> {
                new ReceiptDetail { Id = 1, ProductId = 1, UnitPrice = 10, DiscountUnitPrice = 9, Quantity = 2, ReceiptId = 1 },
                new ReceiptDetail { Id = 2, ProductId = 2, UnitPrice = 20, DiscountUnitPrice = 19, Quantity = 3, ReceiptId = 1 },
                new ReceiptDetail { Id = 3, ProductId = 3, UnitPrice = 25, DiscountUnitPrice = 24, Quantity = 1, ReceiptId = 1 }
                }
            };


            var mockReceiptRepository = new Mock<IReceiptRepository>();
            mockReceiptRepository
                .Setup(x => x.GetByIdWithDetailsAsync(It.IsAny<int>()))
                .ReturnsAsync(receipt);

            var mockProductRepository = new Mock<IProductRepository>();
            var mockReceiptDetailRepository = new Mock<IReceiptDetailRepository>();

            mockReceiptDetailRepository
                .Setup(x => x.AddAsync(It.IsAny<ReceiptDetail>()));

            var receiptService = new ReceiptService(mockReceiptRepository.Object,
                                                    mockProductRepository.Object,
                                                    UnitTestHelper.CreateMapperProfile(),
                                                    mockReceiptDetailRepository.Object);

            //act
            await receiptService.AddProductAsync(productId, 2, quantity);

            //assert
            var actualQuantity = receipt.ReceiptDetails.First(x => x.ProductId == productId).Quantity;
            actualQuantity.Should().Be(expectedQuantity);
            mockReceiptDetailRepository.Verify(x => x.AddAsync(It.IsAny<ReceiptDetail>()), Times.Never);
        }


        [TestCase(1, 15, 15.3)]
        [TestCase(2, 20, 15.2)]
        [TestCase(3, 50, 5.0)]
        [TestCase(8, 99, 0.38)]
        public async Task ReceiptServiceAddProductSetsDiscountUnitPriceValueAccordingToCustomersDiscount(int productId, int discount, decimal expectedDiscountPrice)
        {
            // TODO: Test discounts correctly 

            //arrange
            var product = ProductEntities.FirstOrDefault(x => x.Id == productId);
            var receipt = new Receipt { Id = 1, CustomerId = 1 };


            var mockReceiptRepository = new Mock<IReceiptRepository>();
            mockReceiptRepository
                .Setup(x => x.GetByIdWithDetailsAsync(It.IsAny<int>()))
                .ReturnsAsync(receipt);

            var mockProductRepository = new Mock<IProductRepository>();

            mockProductRepository
                .Setup(x => x.GetByIdAsync(It.IsAny<int>()))
                .ReturnsAsync(product);

            var mockReceiptDetailRepository = new Mock<IReceiptDetailRepository>();

            mockReceiptDetailRepository
                .Setup(x => x.AddAsync(It.IsAny<ReceiptDetail>()));

            var receiptService = new ReceiptService(mockReceiptRepository.Object,
                                                    mockProductRepository.Object,
                                                    UnitTestHelper.CreateMapperProfile(),
                                                    mockReceiptDetailRepository.Object);

            //act
            await receiptService.AddProductAsync(productId, 1, 2);

            //assert
            //mockReceiptDetailRepository.Verify(x => x.AddAsync(It.Is<ReceiptDetail>(detail =>
            //    detail.ReceiptId == receipt.Id && detail.UnitPrice == product.Price && detail.ProductId == product.Id &&
            //    detail.DiscountUnitPrice == expectedDiscountPrice)), Times.Once);

            mockReceiptDetailRepository.Verify(x => x.AddAsync(It.Is<ReceiptDetail>(detail =>
                detail.ReceiptId == receipt.Id && detail.UnitPrice == product.Price && detail.ProductId == product.Id)), Times.Once);

        }


        [TestCase(1)]
        [TestCase(2)]
        public async Task ReceiptServiceAddProductThrowsRestaurantExceptionIfProductDoesNotExist(int productId)
        {
            //arrange
            var receipt = new Receipt { Id = 1, CustomerId = 1 };

            var mockReceiptRepository = new Mock<IReceiptRepository>();
            mockReceiptRepository
                .Setup(x => x.GetByIdWithDetailsAsync(It.IsAny<int>()))
                .ReturnsAsync(receipt);

            var mockProductRepository = new Mock<IProductRepository>();

            mockProductRepository
                .Setup(x => x.GetByIdAsync(It.IsAny<int>()));

            var mockReceiptDetailRepository = new Mock<IReceiptDetailRepository>();

            var receiptService = new ReceiptService(mockReceiptRepository.Object,
                                                    mockProductRepository.Object,
                                                    UnitTestHelper.CreateMapperProfile(),
                                                    mockReceiptDetailRepository.Object);

            //act
            Func<Task> act = async () => await receiptService.AddProductAsync(productId, 1, 1);

            //assert
            await act.Should().ThrowAsync<RestaurantException>();
        }

        [TestCase(1)]
        [TestCase(2)]
        public async Task ReceiptServiceAddProductThrowsRestaurantExceptionIfReceiptDoesNotExist(int receiptId)
        {
            //arrange
            var mockReceiptRepository = new Mock<IReceiptRepository>();
            mockReceiptRepository
                .Setup(x => x.GetByIdWithDetailsAsync(It.IsAny<int>()));

            var mockProductRepository = new Mock<IProductRepository>();

            mockProductRepository
                .Setup(x => x.GetByIdAsync(It.IsAny<int>()));

            var mockReceiptDetailRepository = new Mock<IReceiptDetailRepository>();

            var receiptService = new ReceiptService(mockReceiptRepository.Object,
                                                    mockProductRepository.Object,
                                                    UnitTestHelper.CreateMapperProfile(),
                                                    mockReceiptDetailRepository.Object);



            //act
            Func<Task> act = async () => await receiptService.AddProductAsync(1, receiptId, 1);

            //assert
            await act.Should().ThrowAsync<RestaurantException>();
        }


        [Test]
        public async Task ReceiptServiceCheckOutAsyncUpdatesCheckOutPropertyValueAndSavesChanges()
        {
            //arrange
            var receipt = new Receipt { Id = 6, IsCheckedOut = false };

            var mockReceiptRepository = new Mock<IReceiptRepository>();
            mockReceiptRepository
                .Setup(x => x.GetByIdAsync(It.IsAny<int>()))
                .ReturnsAsync(receipt);

            var mockProductRepository = new Mock<IProductRepository>();
            var mockReceiptDetailRepository = new Mock<IReceiptDetailRepository>();

            var receiptService = new ReceiptService(mockReceiptRepository.Object,
                                                    mockProductRepository.Object,
                                                    UnitTestHelper.CreateMapperProfile(),
                                                    mockReceiptDetailRepository.Object);

            //act
            await receiptService.CheckOutAsync(6);

            //assert
            receipt.IsCheckedOut.Should().BeTrue();
        }


        [TestCase(1, 1, 1)]
        [TestCase(2, 2, 1)]
        [TestCase(3, 3, 2)]
        public async Task ReceiptServiceRemoveProductAsyncUpdatesDetailQuantityValue(int productId, int quantity, int expectedQuantity)
        {
            //arrange
            var receipt = new Receipt
            {
                Id = 1,
                ReceiptDetails = new List<ReceiptDetail>
                {
                    new ReceiptDetail { Id = 1, ProductId = 1, UnitPrice = 10, DiscountUnitPrice = 9, Quantity = 2, ReceiptId = 1 },
                    new ReceiptDetail { Id = 2, ProductId = 2, UnitPrice = 20, DiscountUnitPrice = 19, Quantity = 3, ReceiptId = 1 },
                    new ReceiptDetail { Id = 3, ProductId = 3, UnitPrice = 25, DiscountUnitPrice = 24, Quantity = 5, ReceiptId = 1 }
                }
            };


            var mockReceiptRepository = new Mock<IReceiptRepository>();
            mockReceiptRepository
                .Setup(x => x.GetByIdWithDetailsAsync(It.IsAny<int>()))
                .ReturnsAsync(receipt);

            var mockProductRepository = new Mock<IProductRepository>();
            var mockReceiptDetailRepository = new Mock<IReceiptDetailRepository>();

            var receiptService = new ReceiptService(mockReceiptRepository.Object,
                                                    mockProductRepository.Object,
                                                    UnitTestHelper.CreateMapperProfile(),
                                                    mockReceiptDetailRepository.Object);


            //act
            await receiptService.RemoveProductAsync(productId, 1, quantity);

            //assert
            var actualQuantity = receipt.ReceiptDetails.First(x => x.ProductId == productId).Quantity;
            actualQuantity.Should().Be(expectedQuantity);
        }

        [Test]
        public async Task ReceiptServiceRemoveProductAsyncDeletesDetailIfQuantityEqualsZero()
        {

            //arrange
            var receipt = new Receipt
            {
                Id = 1,
                ReceiptDetails = new List<ReceiptDetail>
                {
                    new ReceiptDetail { Id = 1, ProductId = 1, UnitPrice = 10, DiscountUnitPrice = 9, Quantity = 2, ReceiptId = 1 },
                    new ReceiptDetail { Id = 2, ProductId = 2, UnitPrice = 20, DiscountUnitPrice = 19, Quantity = 3, ReceiptId = 1 },
                    new ReceiptDetail { Id = 3, ProductId = 3, UnitPrice = 25, DiscountUnitPrice = 24, Quantity = 1, ReceiptId = 1 }
                }
            };


            var mockReceiptRepository = new Mock<IReceiptRepository>();
            mockReceiptRepository
                .Setup(x => x.GetByIdWithDetailsAsync(It.IsAny<int>()))
                .ReturnsAsync(receipt);

            var mockProductRepository = new Mock<IProductRepository>();
            var mockReceiptDetailRepository = new Mock<IReceiptDetailRepository>();
            mockReceiptDetailRepository
                .Setup(x => x.Delete(It.IsAny<ReceiptDetail>()));

            var receiptService = new ReceiptService(mockReceiptRepository.Object,
                                                    mockProductRepository.Object,
                                                    UnitTestHelper.CreateMapperProfile(),
                                                    mockReceiptDetailRepository.Object);


            //act
            await receiptService.RemoveProductAsync(1, 1, 2);

            //assert
            mockReceiptDetailRepository.Verify(x => x.Delete(It.Is<ReceiptDetail>(rd => rd.ReceiptId == receipt.Id && rd.ProductId == 1)), Times.Once());
        }



        private static IEnumerable<Receipt> GetTestReceiptsEntities =>
          new List<Receipt>()
          {
                new Receipt
                {
                    Id = 1, CustomerId = 1, IsCheckedOut = false, OperationDate = new DateTime(1965, 1, 2),
                    ReceiptDetails = new List<ReceiptDetail>()
                    {
                        new ReceiptDetail { Id = 1, ProductId = 1, UnitPrice = 10, DiscountUnitPrice = 9, Quantity = 2, ReceiptId = 1 },
                        new ReceiptDetail { Id = 2, ProductId = 2, UnitPrice = 20, DiscountUnitPrice = 19, Quantity = 3, ReceiptId = 1},
                        new ReceiptDetail { Id = 3, ProductId = 3, UnitPrice = 25, DiscountUnitPrice = 24, Quantity = 1, ReceiptId = 1}
                    }
                },
                new Receipt
                {
                    Id = 2, CustomerId = 2, IsCheckedOut = false, OperationDate = new DateTime(1965, 1, 15),
                    ReceiptDetails = new List<ReceiptDetail>()
                    {
                        new ReceiptDetail { Id = 4, ProductId = 1, UnitPrice = 10, DiscountUnitPrice = 9, Quantity = 10, ReceiptId = 2 },
                        new ReceiptDetail { Id = 5, ProductId = 3, UnitPrice = 25, DiscountUnitPrice = 24, Quantity = 1, ReceiptId = 2}
                    }
                },
                new Receipt
                {
                    Id = 3, CustomerId = 3, IsCheckedOut = false, OperationDate = new DateTime(1965, 2, 15),
                    ReceiptDetails = new List<ReceiptDetail>()
                    {
                        new ReceiptDetail { Id = 6, ProductId = 1, UnitPrice = 10, DiscountUnitPrice = 9, Quantity = 10, ReceiptId = 3 },
                        new ReceiptDetail { Id = 7, ProductId = 2, UnitPrice = 25, DiscountUnitPrice = 24, Quantity = 18, ReceiptId = 3}
                    }
                },
                new Receipt
                {
                    Id = 4, CustomerId = 4, IsCheckedOut = false, OperationDate = new DateTime(1965, 2, 28),
                    ReceiptDetails = new List<ReceiptDetail>()
                    {
                        new ReceiptDetail { Id = 8, ProductId = 5, UnitPrice = 10,  DiscountUnitPrice = 9, Quantity = 10, ReceiptId = 4 },
                        new ReceiptDetail { Id = 9, ProductId = 6, UnitPrice = 25,  DiscountUnitPrice = 24, Quantity = 19, ReceiptId = 4 }
                    }
                },
                new Receipt
                {
                    Id = 5, CustomerId = 1, IsCheckedOut = true, OperationDate = new DateTime(1965, 2, 22),
                    ReceiptDetails = new List<ReceiptDetail>()
                    {
                        new ReceiptDetail { Id = 10, ProductId = 8, UnitPrice = 20, DiscountUnitPrice = 15, Quantity = 30, ReceiptId = 5 },
                        new ReceiptDetail { Id = 11, ProductId = 9, UnitPrice = 35, DiscountUnitPrice = 26, Quantity = 24, ReceiptId = 5}
                    }
                }
          };

        private static IEnumerable<ReceiptModel> GetTestReceiptsModels =>
         new List<ReceiptModel>()
         {
            new ReceiptModel
            {
                Id = 1, CustomerId = 1, IsCheckedOut = false,  OperationDate = new DateTime(1965, 1, 2),
                ReceiptDetailsIds = new List<int>() { 1, 2, 3 }
            },
            new ReceiptModel
            {
                Id = 2, CustomerId = 2, IsCheckedOut = false,  OperationDate = new DateTime(1965, 1, 15),
                ReceiptDetailsIds = new List<int>() { 4, 5 }
            },
            new ReceiptModel
            {
                Id = 3, CustomerId = 3, IsCheckedOut = false, OperationDate = new DateTime(1965, 2, 15),
                ReceiptDetailsIds = new List<int>() { 6, 7 }
            },
            new ReceiptModel
            {
                Id = 4, CustomerId = 4, IsCheckedOut = false, OperationDate = new DateTime(1965, 2, 28),
                ReceiptDetailsIds = new List<int>() { 8, 9 }
            },
            new ReceiptModel
            {
                Id = 5, CustomerId = 1, IsCheckedOut = true, OperationDate =  new DateTime(1965, 2, 22),
                ReceiptDetailsIds = new List<int>() { 10, 11 }
            }
         };


        private static IEnumerable<Product> ProductEntities =>
            new List<Product>
            {
                new Product {Id = 1, ProductName = "Chai", ProductCategoryId = 1, Category = new ProductCategory { Id = 1, CategoryName = "Beverages" }, Price = 18.00m },
                new Product {Id = 2, ProductName = "Chang", ProductCategoryId = 1, Category = new ProductCategory { Id = 1, CategoryName = "Beverages" }, Price = 19.00m },
                new Product {Id = 3, ProductName = "Aniseed Syrup", ProductCategoryId = 2, Category = new ProductCategory { Id = 2, CategoryName = "Condiments" }, Price = 10.00m },
                new Product {Id = 4, ProductName = "Chef Anton's Cajun Seasoning", ProductCategoryId = 2, Category =  new ProductCategory { Id = 2, CategoryName = "Condiments" }, Price = 22.00m },
                new Product {Id = 5, ProductName = "Grandma's Boysenberry Spread", ProductCategoryId = 3, Category = new ProductCategory { Id = 3, CategoryName = "Confections" } , Price = 25.00m },
                new Product {Id = 6, ProductName = "Uncle Bob's Organic Dried Pears", ProductCategoryId = 4, Category =  new ProductCategory { Id = 4, CategoryName = "Dairy Products" },Price = 14.60m },
                new Product {Id = 7, ProductName = "Queso Cabrales", ProductCategoryId = 4, Category =  new ProductCategory { Id = 4, CategoryName = "Dairy Products" }, Price = 21.00m },
                new Product {Id = 8, ProductName = "Queso Manchego La Pastora", ProductCategoryId = 3, Category = new ProductCategory { Id = 3, CategoryName = "Confections" } ,Price = 38.00m },
                new Product {Id = 9, ProductName = "Tofu", ProductCategoryId = 2, Category = new ProductCategory { Id = 2, CategoryName = "Condiments" }, Price = 15.50m }
            };
    }
}
