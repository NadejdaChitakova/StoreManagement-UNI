using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace WebApplication1.Models.Domain
{
    public class User : IdentityUser
    {
        public Guid Id { get; set; }
        public string Phone { get; set; }
    }
}
