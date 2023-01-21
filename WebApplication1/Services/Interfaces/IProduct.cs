using Microsoft.AspNetCore.Mvc;
using System.Drawing;
using WebApplication1.Models.Domain;
using WebApplication1.Models.Entity;

namespace WebApplication1.Services.Interfaces
{
    public interface IProduct
    {
        Task<StatusCodeResult> CreateProduct(ProductDTO productDTO);
        ProductDTO FindProductById(Guid productId);
        IFormFile ReadFileFromDB(string PictureName, byte[] Picture, string PictureFormat);
        Task<Image> ConvertImageFromIForm(byte[] arr);
        void UpdateProduct(ProductDTO productDTO);
        List<Product> GetProductByCategory(string categoryId);
        List<Product> GetProducts();
    }
}
