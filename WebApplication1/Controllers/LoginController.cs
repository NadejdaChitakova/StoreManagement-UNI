using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using WebApplication1.Models.Entity;
using WebApplication1.Models;
using WebApplication1.Services.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNet.Identity;
using WebApplication1.Models.Domain;
using Microsoft.EntityFrameworkCore;
using System.Net;

namespace WebApplication1.Controllers
{
    public class LoginController : Controller
    {
        private readonly ILogin _loginService;
        private readonly IMapper _mapper;

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
        public async Task<IActionResult> Login(LoginVM model) //TODO: MAKE LOGGIN TO WORK
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
                    return RedirectToAction("Index", "Home");
                }
                else
                {
                    ModelState.AddModelError(string.Empty, "Invalid Login Attempt");
                }
            }
            return View(model);
        }
    }
}
