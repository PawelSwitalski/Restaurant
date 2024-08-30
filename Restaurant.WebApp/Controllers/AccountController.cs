using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Restaurant.WebApp.Filters;
using Restaurant.WebApp.Models.ViewModels;

namespace Restaurant.WebApp.Controllers
{
    [Authorize]
    public class AccountController : Controller
    {
        private readonly UserManager<IdentityUser> userManager;
        private readonly SignInManager<IdentityUser> signInManager;

        public AccountController(UserManager<IdentityUser> userManager, SignInManager<IdentityUser> signInManager)
        {
            this.userManager = userManager;
            this.signInManager = signInManager;
        }


        [HttpGet]
        [AllowAnonymous]
        [AnonymousOnly("AuthenticatedIndex", "Account")]
        public IActionResult Index()
        {
            return View("AnonymousIndex");
        }

        [HttpGet]
        public IActionResult AuthenticatedIndex()
        {
            return View("Index");
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginViewModel loginModel)
        {
            if (this.ModelState.IsValid)
            {
                IdentityUser user = await this.userManager.FindByEmailAsync(loginModel.Email);

                if (user != null)
                {
                    await this.signInManager.SignOutAsync();

                    if ((await this.signInManager.PasswordSignInAsync(user, loginModel.Password, false, false)).Succeeded)
                    {
                        return this.RedirectToAction("AuthenticatedIndex", "Account");
                    }
                }

                this.ModelState.AddModelError(string.Empty, "Invalid name or password.");
            }

            return this.View(loginModel);
        }
    }
}
