using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using WebApplication1.Models;
using WebApplication1.Models.Domain;
using WebApplication1.Models.Entity;
using WebApplication1.Services.Interfaces;

namespace WebApplication1.Controllers
{
    public class RegistrationController : Controller
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly SignInManager<IdentityUser> _signInManager;

        public RegistrationController(UserManager<IdentityUser> userManager, SignInManager<IdentityUser> signInManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
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
            if (ModelState.IsValid)
            {
                var user = new User
                {
                    Id = new Guid(),
                    Email = addUserVM.Email,
                    UserName = addUserVM.Username,
                    PasswordHash = addUserVM.Password,
                    Phone = addUserVM.Phone,
                };

                var result = await _userManager.CreateAsync(user, user.PasswordHash);
                if (result.Succeeded)
                {
                    await _signInManager.SignInAsync(user, false);
                }
                return RedirectToAction("RegisterSuccessfuly", "Registration", new { area = "" });
            }
            return View();
        }
    }
}
