using Microsoft.AspNetCore.Mvc;
using WebApplication1.Models;
using WebApplication1.Models.Domain;

namespace WebApplication1.Controllers
{
    public class RegistrationController : Controller
    {
        [HttpGet]
        public IActionResult Add()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Add(AddUserVM addUserVM)
        {
            var user = new User()
            {
                Id = Guid.NewGuid(),
                Username = addUserVM.Username,
                Email = addUserVM.Email,
                Password = addUserVM.Password,
                Phone = addUserVM.Phone
            };
            return RedirectToAction("Add");
        }
    }
}
