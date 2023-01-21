using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using WebApplication1.Data;
using WebApplication1.Models.Domain;
using WebApplication1.Models.Entity;
using WebApplication1.Services.Interfaces;

namespace WebApplication1.Services
{
    public class RegisterService : IRegister
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly SignInManager<IdentityUser> _signInManager;
        private readonly ApplicationDBContext _applicationDBContext;
        private readonly IFileService _fileService;

        public RegisterService(UserManager<IdentityUser> userManager, SignInManager<IdentityUser> signInManager, ApplicationDBContext applicationDBContext, IFileService fileService)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _applicationDBContext = applicationDBContext;
            _fileService = fileService;
        }

        public async Task<StatusCodeResult> RegisterAsync(RegisterDTO registerDTO)
        {
            if (registerDTO.UserName == null || registerDTO.PasswordHash == null || registerDTO.Email == null)
            {
                _fileService.WriteOnFile(DateTime.Now.ToString() + "The username, password or email has not value");
                return new BadRequestResult();
            }

            var userInDB = _applicationDBContext.Users.Where(x => x.Email == registerDTO.Email);

            if (userInDB == null)
            {
                _fileService.WriteOnFile(DateTime.Now.ToString() + "The user exist in the database");
                return new NotFoundResult();
            }
            var user = new User
            {
                Id = registerDTO.Id,
                UserName = registerDTO.UserName,
                Email = registerDTO.Email,
                PasswordHash = registerDTO.PasswordHash,
                Phone = registerDTO.Phone
            };

            var result = await _userManager.CreateAsync(user, user.PasswordHash);
            var statusCode = StatusCodes.Status400BadRequest;
            if (result.Succeeded)
            {
                statusCode = StatusCodes.Status200OK;
            }
            else
            {
            _fileService.WriteOnFile(DateTime.Now.ToString() + "The user exist in the database");
            }
            return new StatusCodeResult(statusCode);
        }
    }
}
