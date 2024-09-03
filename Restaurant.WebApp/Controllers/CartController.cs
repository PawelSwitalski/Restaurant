using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Migrations;
using Restaurant.Business.Interfaces;
using Restaurant.Business.Models;
using Restaurant.Data.Entities;
using Restaurant.WebApp.Models;
using Restaurant.WebApp.Models.ViewModels;

namespace Restaurant.WebApp.Controllers
{
    public class CartController : Controller
    {
        private readonly IProductService productService;

        public CartController(IProductService productService, Cart cart)
        {
            this.productService = productService;
            this.Cart = cart;
        }

        public Cart Cart { get; set; }

        [HttpGet]
        public IActionResult Index(string returnUrl)
        {
            return this.View(new CartViewModel
            {
                ReturnUrl = returnUrl ?? "/",
                Cart = this.Cart,
            });
        }

        [HttpPost]
        public async Task<IActionResult> Index(int productId, string returnUrl)
        {
            ProductModel productModel = await this.productService.GetByIdAsync(productId);

            if (productModel != null)
            {
                this.Cart.AddItem(productModel, 1);

                return this.View(new CartViewModel
                {
                    Cart = this.Cart,
                    ReturnUrl = returnUrl,
                });
            }

            return this.RedirectToAction("Index", "Home");
        }

        [HttpPost]
        [Route("Cart/Remove")]
        public IActionResult Remove(long productId, string returnUrl)
        {
            this.Cart.RemoveLine(this.Cart.Lines.First(cl => cl.Product.Id == productId).Product);
            return this.View("Index", new CartViewModel
            {
                Cart = this.Cart,
                ReturnUrl = returnUrl ?? "/",
            });
        }
    }
}
