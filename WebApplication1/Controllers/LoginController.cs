using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using WebApplication1.Models;
using WebApplication1.Models.Entity;
using WebApplication1.Services.Interfaces;

namespace WebApplication1.Controllers
{
    public class LoginController : Controller
    {
        private readonly ILogin _loginService;
        //private readonly IMapper _mapper;

        public LoginController(ILogin loginService)
        {
            _loginService = loginService;
        }

        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public async Task<ActionResult> Login(LoginVM model)
        {
            if (ModelState.IsValid)
            {
                var user = new LoginDTO
                {
                    Email = model.Email,
                    Password = model.Password,
                };

                var result = await _loginService.Login(user);

                if (result.StatusCode == StatusCodes.Status200OK)
                {
                    return RedirectToAction("Index", "Products", new { area = "" });
                }

            }
            return View(model);
        }
    }
}
