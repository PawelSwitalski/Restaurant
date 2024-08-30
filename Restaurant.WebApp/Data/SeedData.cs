using Restaurant.Data.Data;
using Restaurant.Data.Entities;

namespace Restaurant.WebApp.Data
{
    public static class SeedData
    {
        public static void EnsurePopulated(IApplicationBuilder app)
        {
            if (app == null)
            {
                throw new ArgumentNullException(nameof(app));
            }

            RestaurantDbContext context = app.ApplicationServices.CreateScope().ServiceProvider.GetRequiredService<RestaurantDbContext>();

            if (!context.ProductCategories.Any())
            {
                context.ProductCategories.AddRange(
                    new ProductCategory 
                    {
                        CategoryName = "Salad"
                    },
                    new ProductCategory
                    {
                        CategoryName = "Pizza"
                    });

                context.SaveChanges();
            }

            if (!context.Products.Any())
            {
                context.Products.AddRange(
                    new Product
                    {
                        ProductCategoryId = 2,
                        ProductName = "Salami",
                        ProductDescription = "Pizza Salami witch cheese and onion stripes.",
                        Price = 22.5m
                    },
                    new Product
                    {
                        ProductCategoryId = 2,
                        ProductName = "Hawaii",
                        ProductDescription = "Pizza with ananas and ham.",
                        Price = 20.0m
                    },
                    new Product
                    {
                        ProductCategoryId = 2,
                        ProductName = "Pizza Margherita",
                        ProductDescription = "Cheese and ketchup",
                        Price = 15.0m
                    },
                    new Product
                    {
                        ProductCategoryId = 2,
                        ProductName = "Pizza Capricciosa",
                        ProductDescription = "Pizza with champignons and ham.",
                        Price = 20.0m
                    },
                    new Product
                    {
                        ProductCategoryId = 2,
                        ProductName = "Pizza Prosciutto",
                        ProductDescription = "Pizza with cured ham, cherry tomato, rocket and parmesan flakes",
                        Price = 30.0m
                    },

                    new Product
                    {
                        ProductCategoryId = 1,
                        ProductName = "Greek salad",
                        ProductDescription = "Salad with mixed lettuce, cucumber, tomato, feta cheese, onion and vinaigrette dressing",
                        Price = 10
                    },
                    new Product
                    {
                        ProductCategoryId = 1,
                        ProductName = "Prosciutto Salad",
                        ProductDescription = "Salad with mixed lettuce, rocket, mozzarella cheese, cured ham, cherry tomato, peach, roasted sunflower seeds, parmesan flakes and balsamic sauce",
                        Price = 13.4m
                    }
                    );

                context.SaveChanges();
            }
        }
    }
}
