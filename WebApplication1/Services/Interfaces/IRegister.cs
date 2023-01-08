using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using WebApplication1.Models.Entity;

namespace WebApplication1.Services.Interfaces
{
    public interface IRegister
    {
        Task<StatusCodeResult> RegisterAsync(RegisterDTO registerDTO);
    }
}
