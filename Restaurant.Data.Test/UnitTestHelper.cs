using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Restaurant.Business;
using Restaurant.Data.Data;
using Restaurant.Data.Entities;

namespace Restaurant.Data.Test
{
    public static class UnitTestHelper
    {
        public static DbContextOptions<RestaurantDbContext> GetUnitTestDbOptions()
        {
            var options = new DbContextOptionsBuilder<RestaurantDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;

            using (var context = new RestaurantDbContext(options))
            {
                SeedData(context);
            }

            return options;
        }

        public static IMapper CreateMapperProfile()
        {
            var myProfile = new AutomapperProfile();
            var configuration = new MapperConfiguration(cfg => cfg.AddProfile(myProfile));

            return new Mapper(configuration);
        }

        public static void SeedData(RestaurantDbContext context)
        {
            context.ProductCategories.AddRange(
                new ProductCategory { Id = 1, CategoryName = "Dairy products" },
                new ProductCategory { Id = 2, CategoryName = "Fruit juices" });
            context.Products.AddRange(
                new Product { Id = 1, ProductCategoryId = 1, ProductName = "Milk", ProductDescription = "2% fat", Price = 40, ImagePath = "products/Pizzas/Pepperoni.png" },
                new Product { Id = 2, ProductCategoryId = 2, ProductName = "Orange juice", ProductDescription = "expired date 11.2025", Price = 20, ImagePath = "products/Pizzas/Pepperoni.png" });
            context.Receipts.AddRange(
                new Receipt { Id = 1, CustomerId = 1, OperationDate = new DateTime(2021, 7, 5), IsCheckedOut = true },
                new Receipt { Id = 2, CustomerId = 1, OperationDate = new DateTime(2021, 8, 10), IsCheckedOut = true },
                new Receipt { Id = 3, CustomerId = 2, OperationDate = new DateTime(2021, 10, 15), IsCheckedOut = false });
            context.ReceiptsDetails.AddRange(
                new ReceiptDetail { Id = 1, ReceiptId = 1, ProductId = 1, UnitPrice = 40, DiscountUnitPrice = 32, Quantity = 3 },
                new ReceiptDetail { Id = 2, ReceiptId = 1, ProductId = 2, UnitPrice = 20, DiscountUnitPrice = 16, Quantity = 1 },
                new ReceiptDetail { Id = 3, ReceiptId = 2, ProductId = 2, UnitPrice = 20, DiscountUnitPrice = 32, Quantity = 2 },
                new ReceiptDetail { Id = 4, ReceiptId = 3, ProductId = 1, UnitPrice = 40, DiscountUnitPrice = 36, Quantity = 2 },
                new ReceiptDetail { Id = 5, ReceiptId = 3, ProductId = 2, UnitPrice = 20, DiscountUnitPrice = 18, Quantity = 5 });
            context.SaveChanges();
        }
    }
}
