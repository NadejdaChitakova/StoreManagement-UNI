using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using WebApplication1.Models.Entity;
using WebApplication1.Models;
using WebApplication1.Services.Interfaces;

namespace WebApplication1.Controllers
{
    public class LoginController : Controller
    {
        private readonly ILogin _loginService;
        private readonly IMapper _mapper;
        public LoginController(ILogin loginService, IMapper mapper)
        {
            _loginService = loginService;
            _mapper = mapper;
        }

        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(AddUserVM addUserVM) //TODO: MAKE LOGGIN TO WORK
        {
            var user = new LoginDTO
            {
                Username = addUserVM.Username,
                Password = addUserVM.Password
            };

            await _loginService.Login(user);
            return RedirectToAction(actionName: "Index", controllerName: "Home");
        }
    }
}
