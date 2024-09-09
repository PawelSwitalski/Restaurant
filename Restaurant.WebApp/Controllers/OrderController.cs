using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Restaurant.Business.Interfaces;
using Restaurant.Business.Models;
using Restaurant.WebApp.Models;

namespace Restaurant.WebApp.Controllers
{
    public class OrderController : Controller
    {
        private readonly IReceiptService receiptService;

        private readonly Cart cart;

        private readonly UserManager<IdentityUser> userManager;

        public OrderController(IReceiptService receiptService, Cart cart)
        {
            this.receiptService = receiptService;
            this.cart = cart;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult OrdersHistory()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Checkout()
        {
            if (!this.cart.Lines.Any())
            {
                this.ModelState.AddModelError(key: string.Empty, errorMessage: "Sorry, your cart is empty!");
            }

            if (this.ModelState.IsValid)
            {
                ReceiptModel model = new ReceiptModel();
                model.CustomerId = 1;

                await receiptService.AddAsync(model);

                //foreach (CartLine line in this.cart.Lines)
                //{
                //    await receiptService.AddProductAsync(line.Product.Id, model.Id, line.Quantity);
                //}

                this.cart.Clear();
                return this.View(viewName: "Completed");
            }

            return this.View();
        }
    }
}
