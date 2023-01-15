using Microsoft.AspNetCore.Mvc;
using WebApplication1.Models;
using WebApplication1.Models.Entity;
using WebApplication1.Services.Interfaces;

namespace WebApplication1.Controllers
{
    public class RegistrationController : Controller
    {
        private readonly IRegister _register;
        public RegistrationController(IRegister register)
        {
            _register = register;
        }

        [HttpGet]
        public IActionResult RegisterSuccessfuly()
        {
            return View();
        }

        [HttpGet]
        public IActionResult Add()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Add(AddUserVM addUserVM)
        {
            if (ModelState.IsValid) // TODO: show error msg when username exist in db
            {
                var user = new RegisterDTO
                {
                    Id = new Guid(),
                    Email = addUserVM.Email,
                    UserName = addUserVM.Username,
                    PasswordHash = addUserVM.Password,
                    Phone = addUserVM.Phone,
                };

                var result = await _register.RegisterAsync(user);

                if (result.StatusCode.Equals(StatusCodes.Status200OK))
                {
                    return RedirectToAction("RegisterSuccessfuly", "Registration", new { area = "" });
                }
            }
            return View();
        }
    }
}
