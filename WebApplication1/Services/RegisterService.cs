using AutoMapper;
using WebApplication1.Data;
using WebApplication1.Models.Domain;
using WebApplication1.Models.Entity;
using WebApplication1.Services.Interfaces;

namespace WebApplication1.Services
{
    public class RegisterService : IRegister
    {
        private readonly ApplicationDBContext _applicationDBContext;
        private readonly IMapper _mapper;
        public RegisterService(ApplicationDBContext applicationDBContext, IMapper mapper)
        {
            _applicationDBContext = applicationDBContext;
            _mapper = mapper;
        }

        public async Task RegisterAsync(RegisterDTO registerDTO)
        {
            var user = new User
            {
                Id = registerDTO.Id,
                Username = registerDTO.Username,
                Email = registerDTO.Email,
                Password = registerDTO.Password,
                Phone = registerDTO.Phone
            };

            if (registerDTO.Username == null || registerDTO.Password == null  || registerDTO.Email == null)
            {
                return;
            }

            await _applicationDBContext.Users.AddAsync(user);
            await _applicationDBContext.SaveChangesAsync();
        }
    }
}
