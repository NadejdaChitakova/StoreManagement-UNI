using Microsoft.AspNetCore.Mvc;
using System.Drawing;
using WebApplication1.Models.Domain;
using WebApplication1.Models.Entity;

namespace WebApplication1.Services.Interfaces
{
    public interface IProduct
    {
        List<ProductDTO> GetProducts();
        List<ProductDTO> GetProductByCategory(string categoryId);
        ProductDTO FindProductById(Guid productId);
        Task<StatusCodeResult> CreateProduct(ProductDTO productDTO);
        void DeleteProduct(Guid productId);
        void UpdateProduct(ProductDTO productDTO);
        Task<Image> ConvertImageFromIForm(byte[] arr);
        IFormFile ReadFileFromDB(string PictureName, byte[] Picture, string PictureFormat);
    }
}
