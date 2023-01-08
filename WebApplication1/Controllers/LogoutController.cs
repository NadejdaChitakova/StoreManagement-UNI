using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace WebApplication1.Controllers
{
    public class LogoutController : Controller
    {
        private readonly SignInManager<IdentityUser> _signInManager;

        public LogoutController(SignInManager<IdentityUser> signInManager)
        {
            _signInManager = signInManager;
        }

        public void Index()
        {

        }

        public async Task<IActionResult> OnPostLogOutAsync()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction("Login", "Login", new { area = "" });
        }
        public IActionResult OnPostDontLogOut()
        {
            return RedirectToAction("Home", "Index", new { area = "" });
        }
    }
}
