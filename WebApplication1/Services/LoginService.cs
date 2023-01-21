using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using WebApplication1.Data;
using WebApplication1.Models.Entity;
using WebApplication1.Services.Interfaces;

namespace WebApplication1.Services
{
    public class LoginService : ILogin
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly SignInManager<IdentityUser> _signInManager;
        private readonly ApplicationDBContext _applicationDBContext;
        private readonly IFileService _fileService;
        public static string loggedIn = null;
        public LoginService(UserManager<IdentityUser> userManager, SignInManager<IdentityUser> signInManager, ApplicationDBContext applicationDBContext, IFileService fileService)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _applicationDBContext = applicationDBContext;
            _signInManager.SignOutAsync();
            _fileService = fileService;
        }

        public async Task<StatusCodeResult> Login(LoginDTO loginDTO)
        {
            var statusCode = StatusCodes.Status401Unauthorized;

            if (loginDTO.Email == null || loginDTO.Password == null)
            {
                _fileService.WriteOnFile(DateTime.Now.ToString() + "The email or the password has null value");
                return new BadRequestResult();
            }
            var userInDb = _applicationDBContext.Users.Where(x => x.Email == loginDTO.Email).FirstOrDefault();

            if (userInDb == null)
            {
                _fileService.WriteOnFile(DateTime.Now.ToString() + "The user doesnt exist in the database");
                return new NotFoundResult();
            }
            userInDb.EmailConfirmed = true;
            _applicationDBContext.SaveChanges();
            var result = await _signInManager.PasswordSignInAsync(userInDb,
                           loginDTO.Password, true, true);

            if (result.Succeeded)
            {
                await _signInManager.SignInAsync(userInDb, true);
                statusCode = StatusCodes.Status200OK;
                loggedIn = "true";
            }
            else
            {
                _fileService.WriteOnFile(DateTime.Now.ToString() + "The email or the password are incorect");
                await _signInManager.SignOutAsync();
            }

            return new StatusCodeResult(statusCode);
        }
    }
}
