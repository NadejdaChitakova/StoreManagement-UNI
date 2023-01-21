using Microsoft.AspNetCore.Mvc;
using WebApplication1.Services;

namespace WebApplication1.Controllers
{
    public class LogoutController : Controller
    {
        public LogoutController()
        {
        }

        public async Task<IActionResult> Index()
        {
            LoginService.loggedIn = null;
            return View();
        }
    }
}
