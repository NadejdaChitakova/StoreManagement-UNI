using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using WebApplication1.Data;
using WebApplication1.Models.Domain;
using WebApplication1.Models.Entity;
using WebApplication1.Services.Interfaces;

namespace WebApplication1.Services
{
    public class LoginService : ILogin
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly SignInManager<IdentityUser> _signInManager;
        private readonly ApplicationDBContext _applicationDBContext;
        public static string loggedUser = null;
        public LoginService(UserManager<IdentityUser> userManager, SignInManager<IdentityUser> signInManager, ApplicationDBContext applicationDBContext)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _applicationDBContext = applicationDBContext;
        }

        public async Task<StatusCodeResult> Login(LoginDTO loginDTO)
        {
            if (loginDTO.Email == null || loginDTO.Password == null)
            {
                return new BadRequestResult();
            }
            var userInDb = _applicationDBContext.Users.Where(x => x.UserName == loginDTO.Email).FirstOrDefault();

            var result = await _signInManager.PasswordSignInAsync(userInDb,
                           userInDb.PasswordHash, true, true);
            var statusCode = StatusCodes.Status200OK;

            if (userInDb == null)
            {
                return new NotFoundResult();
            }

            return new StatusCodeResult(statusCode);
        }
    }
}
