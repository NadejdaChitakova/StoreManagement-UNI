using System.ComponentModel.DataAnnotations;

namespace WebApplication1.Models
{
    public class AddUserVM
    {
        public Guid Id { get; set; }

        [Required]
        [StringLength(15, ErrorMessage = "{0} length must be between {2} and {1}.", MinimumLength = 5)]
        public string Username { get; set; }

        [Required]
        [DataType(DataType.EmailAddress)]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        [StringLength(20, ErrorMessage = "{0} length must be between {2} and {1}.", MinimumLength = 6)]
        [RegularExpression(@"^(?=.*[A-Za-z])(?=.*\d)(?=.*[@$!%*#?&])[A-Za-z\d@$!%*#?&]{8,}$", ErrorMessage = "Please enter  1 upper char, 1 number and 1 special char")]
        public string Password { get; set; }

        public string Phone { get; set; }
    }
}
