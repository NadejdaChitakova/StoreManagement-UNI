using Microsoft.AspNetCore.Mvc;
using WebApplication1.Models.Entity;

namespace WebApplication1.Services.Interfaces
{
    public interface ILogin
    {
        Task<StatusCodeResult> Login(LoginDTO loginDTO);
    }
}
