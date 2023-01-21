using System.ComponentModel.DataAnnotations;
using System.Drawing;

namespace WebApplication1.Models
{
    public class ProductVM
    {
        public Guid Id { get; set; }
        [Required]
        [StringLength(50)]
        public string Name { get; set; }

        [StringLength(2000)]
        public string Description { get; set; }
        [Required]
        [Display(Name = "Buy price")]
        public decimal BuyPrice { get; set; }
        [Required]
        [Display(Name = "Sell price")]
        public decimal SellPrice { get; set; }
        public IFormFile Picture { get; set; }
        public string ImageImage { get; set; }
        public string CategoryId { get; set; }
        [Display(Name = "Category name")]
        public string CategoryName { get; set; }
        [Required]
        [Range(1, int.MaxValue, ErrorMessage = "Please enter a value bigger than {1}")]
        [Display(Name = "Product count")]
        public int ProductCount { get; set; }
        [Display(Name = "Product code")]
        public decimal ProductCode { get; set; }
    }
}
