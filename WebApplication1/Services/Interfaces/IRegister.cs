using WebApplication1.Models.Entity;

namespace WebApplication1.Services.Interfaces
{
    public interface IRegister
    {
        Task RegisterAsync(RegisterDTO registerDTO);
    }
}
