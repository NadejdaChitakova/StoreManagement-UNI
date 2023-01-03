using WebApplication1.Models.Entity;

namespace WebApplication1.Services.Interfaces
{
    public interface ILogin
    {
        Task Login(LoginDTO loginDTO);
    }
}
