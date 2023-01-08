using AutoMapper;
using WebApplication1.Data;
using WebApplication1.Models.Domain;
using WebApplication1.Models.Entity;
using WebApplication1.Services.Interfaces;

namespace WebApplication1.Services
{
    public class LoginService : ILogin
    {
        private readonly ApplicationDBContext _applicationDBContext;
        private readonly IMapper _mapper;
        public static string loggedUser;
        public LoginService(ApplicationDBContext applicationDBContext, IMapper mapper)
        {
            _applicationDBContext = applicationDBContext;
            _mapper = mapper;
        }

        public async Task Login(LoginDTO loginDTO)
        {
            if (loginDTO.Username == null || loginDTO.Password == null)
            {
                return;
            }
            var user = new User();// _applicationDBContext.Users.Where(x => x.UserName == loginDTO.Username && x.Password == loginDTO.Password).FirstOrDefault();

            if (user == null)
            {
                return;
            }
            loggedUser = user.UserName;
        }
    }
}
