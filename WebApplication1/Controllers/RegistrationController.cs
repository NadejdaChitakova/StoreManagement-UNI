using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using WebApplication1.Models;
using WebApplication1.Models.Domain;
using WebApplication1.Models.Entity;
using WebApplication1.Services.Interfaces;

namespace WebApplication1.Controllers
{
    public class RegistrationController : Controller
    {
        private readonly IRegister _registerService;
        private readonly IMapper _mapper;
        public RegistrationController(IRegister registerService, IMapper mapper)
        {
            _registerService= registerService;
            _mapper= mapper;
        }

        [HttpGet]
        public IActionResult Add()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Add(AddUserVM addUserVM)
        {
            var user = new RegisterDTO
            {
                Id = new Guid(),
                Email = addUserVM.Email,
                Username = addUserVM.Username,
                Password = addUserVM.Password,
                Phone = addUserVM.Phone,
            };
           
            await _registerService.RegisterAsync(user);
            return RedirectToAction("Add");
        }
    }
}
