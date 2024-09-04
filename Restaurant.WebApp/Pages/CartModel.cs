using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Migrations;
using Restaurant.Data.Entities;
using Restaurant.WebApp.Models;
using Restaurant.Business.Interfaces;
using Restaurant.Business.Models;

namespace Restaurant.WebApp.Pages
{
    public class CartModel : PageModel
    {
        private IProductService productService;
        public CartModel(IProductService productService, Cart cartService)
        {
            this.productService = productService;
            Cart = cartService;
        }
        public Cart Cart { get; set; }
        public string ReturnUrl { get; set; } = "/";
        public void OnGet(string returnUrl)
        {
            ReturnUrl = returnUrl ?? "/";
        }
        public async Task<IActionResult> OnPost(int productId, string returnUrl)
        {
            ProductModel? product = await productService.GetByIdAsync(productId);
                
            if (product != null)
            {
                Cart.AddItem(product, 1);
            }
            return RedirectToPage(new { returnUrl = returnUrl });
        }
        public IActionResult OnPostRemove(long productId, string returnUrl)
        {
            Cart.RemoveLine(Cart.Lines.First(cl =>
            cl.Product.Id == productId).Product);
            return RedirectToPage(new { returnUrl = returnUrl });
        }
    }
}
