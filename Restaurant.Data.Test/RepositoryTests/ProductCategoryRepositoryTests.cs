using Restaurant.Data.Data;
using Restaurant.Data.Entities;
using Restaurant.Data.Repositories;

namespace Restaurant.Data.Test.RepositoryTests
{
    [TestFixture]
    public class ProductCategoryRepositoryTests
    {
        [TestCase(1)]
        [TestCase(2)]
        public async Task ProductCategoryRepositoryGetByIdAsyncReturnsSingleValue(int id)
        {
            using var context = new RestaurantDbContext(UnitTestHelper.GetUnitTestDbOptions());

            var productCategoryRepository = new ProductCategoryRepository(context);
            var productCategory = await productCategoryRepository.GetByIdAsync(id);

            var expected = ExpectedProductCategories.FirstOrDefault(x => x.Id == id);

            Assert.That(productCategory, Is.EqualTo(expected).Using(new ProductCategoryEqualityComparer()), message: "GetByIdAsync method works incorrect");
        }

        [Test]
        public async Task ProductCategoryRepositoryGetAllAsyncReturnsAllValues()
        {
            using var context = new RestaurantDbContext(UnitTestHelper.GetUnitTestDbOptions());

            var productCategoryRepository = new ProductCategoryRepository(context);
            var productCategories = await productCategoryRepository.GetAllAsync();

            Assert.That(productCategories, Is.EqualTo(ExpectedProductCategories).Using(new ProductCategoryEqualityComparer()), message: "GetAllAsync method works incorrect");
        }

        [Test]
        public async Task ProductCategoryRepositoryAddAsyncAddsValueToDatabase()
        {
            using var context = new RestaurantDbContext(UnitTestHelper.GetUnitTestDbOptions());

            var productCategoryRepository = new ProductCategoryRepository(context);
            var productCategory = new ProductCategory { Id = 3, CategoryName = "Fruits" };

            await productCategoryRepository.AddAsync(productCategory);
            await context.SaveChangesAsync();

            Assert.That(context.ProductCategories.Count(), Is.EqualTo(3), message: "AddAsync method works incorrect");
        }

        [Test]
        public async Task ProductCategoryRepositoryDeleteByIdAsyncDeletesEntity()
        {
            using var context = new RestaurantDbContext(UnitTestHelper.GetUnitTestDbOptions());

            var productCategoryRepository = new ProductCategoryRepository(context);

            await productCategoryRepository.DeleteByIdAsync(1);
            await context.SaveChangesAsync();

            Assert.That(context.ProductCategories.Count(), Is.EqualTo(1), message: "DeleteByIdAsync works incorrect");
        }

        [Test]
        public async Task ProductCategoryRepositoryUpdateUpdatesEntity()
        {
            using var context = new RestaurantDbContext(UnitTestHelper.GetUnitTestDbOptions());

            var productCategoryRepository = new ProductCategoryRepository(context);
            var productCategory = new ProductCategory
            {
                Id = 1,
                CategoryName = "Dairy food"
            };

            productCategoryRepository.Update(productCategory);
            await context.SaveChangesAsync();

            Assert.That(productCategory, Is.EqualTo(new ProductCategory
            {
                Id = 1,
                CategoryName = "Dairy food"
            }).Using(new ProductCategoryEqualityComparer()), message: "Update method works incorrect");
        }

        private static IEnumerable<ProductCategory> ExpectedProductCategories =>
            new[]
            {
                new ProductCategory { Id = 1, CategoryName = "Dairy products" },
                new ProductCategory { Id = 2, CategoryName = "Fruit juices" }
            };
    }
}
