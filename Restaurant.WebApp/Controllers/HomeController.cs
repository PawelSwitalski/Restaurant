using Microsoft.AspNetCore.Mvc;
using Restaurant.Business.Interfaces;
using Restaurant.Business.Models;
using Restaurant.WebApp.Models;
using System.Diagnostics;

namespace Restaurant.WebApp.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IProductService _productService;

        public HomeController(ILogger<HomeController> logger, IProductService productService)
        {
            _logger = logger;
            _productService = productService;
        }

        public async Task<ActionResult<IEnumerable<ProductModel>>> Index()
        {
            IEnumerable<ProductModel> products = await _productService.GetAllAsync();
            return View(products);

            //return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
