﻿using System.ComponentModel.DataAnnotations;

namespace WebApplication1.Models
{
    public class LoginVM
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }
        [Required]
        // [DataType(DataType.Password)]
        public string Password { get; set; }

    }
}
