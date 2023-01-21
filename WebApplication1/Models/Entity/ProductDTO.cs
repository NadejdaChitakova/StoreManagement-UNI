using System.Drawing;

namespace WebApplication1.Models.Entity
{
    public class ProductDTO
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public decimal BuyPrice { get; set; }
        public decimal SellPrice { get; set; }
        public int ProductCount { get; set; }
        public string PictureName { get; set; }
        public byte[] Picture { get; set; }
        public string PictureFormat { get; set; }
        public string CategoryId { get; set; }
        public decimal ProductCode { get; set; }
        public Image ImageImg { get; set; }
    }
}
