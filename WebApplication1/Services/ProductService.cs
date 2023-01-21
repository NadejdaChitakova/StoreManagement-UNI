using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections;
using System.Drawing;
using System.Net.Mime;
using System.Xml.Linq;
using WebApplication1.Data;
using WebApplication1.Models;
using WebApplication1.Models.Domain;
using WebApplication1.Models.Entity;
using WebApplication1.Services.Interfaces;

namespace WebApplication1.Services
{
    public class ProductService : IProduct
    {
        private readonly ApplicationDBContext _applicationDBContext;

        public ProductService(ApplicationDBContext applicationDBContext)
        {
            _applicationDBContext = applicationDBContext;
        }
        public async Task<StatusCodeResult> CreateProduct(ProductDTO productDTO)
        {
            var codeIsUnique = _applicationDBContext.Product.Where(x => x.ProductCode.Equals(productDTO.ProductCode)).SingleOrDefault();

            if (codeIsUnique != null)
            {
                return new StatusCodeResult(StatusCodes.Status406NotAcceptable);
            }

            var product = MapDTOToEntity(productDTO);

            _applicationDBContext.Product.Add(product);
            _applicationDBContext.SaveChanges();

            return new StatusCodeResult(StatusCodes.Status201Created);
        }

        public ProductDTO FindProductById(Guid productId)
        {
            var product = _applicationDBContext.Product.Where(x => x.Id == productId).SingleOrDefault();
            return MapEntityToDTO(product);
        }

        public IFormFile ReadFileFromDB(string PictureName, byte[] Picture, string PictureFormat)
        {
            var stream1 = new MemoryStream(Picture);

            IFormFile file1 = new FormFile(stream1, 0, Picture.Length, PictureName, PictureName);

            using (var stream = new MemoryStream(Picture))
            {
                var file = new FormFile(stream, 0, Picture.Length, PictureName, PictureName)
                {
                    Headers = new HeaderDictionary(),
                    ContentType = PictureFormat,
                };

                System.Net.Mime.ContentDisposition cd = new System.Net.Mime.ContentDisposition
                {
                    FileName = file.FileName
                };
                file.ContentDisposition = cd.ToString();
                stream1.Close();
                stream1.Dispose();
                return file;
            }
            return file1;
        }

        public async Task<Image> ConvertImageFromIForm(byte[] arr)
        {
            using (var ms = new MemoryStream(arr))
            {
                return Image.FromStream(ms);
            }

        }
        private Product MapDTOToEntity(ProductDTO dto)
        {
            return new Product()
            {
                Id = dto.Id,
                Name = dto.Name,
                Description = dto.Description,
                BuyPrice = dto.BuyPrice,
                SellPrice = dto.SellPrice,
                CategoryId = dto.CategoryId,
                PictureName = dto.PictureName,
                //Picture = dto.Picture, link to file (url)
                PictureFormat = dto.PictureFormat,
                ProductCode = dto.ProductCode,
                ProductCount = dto.ProductCount
            };
        }

        private ProductDTO MapEntityToDTO(Product product)
        {
            return new ProductDTO()
            {
                Id = product.Id,
                Name = product.Name,
                Description = product.Description,
                BuyPrice = product.BuyPrice,
                SellPrice = product.SellPrice,
                CategoryId = product.CategoryId,
                PictureName = product.PictureName,
                Picture = product.Picture,
                PictureFormat = product.PictureFormat,
                ProductCode = product.ProductCode,
                ProductCount = product.ProductCount
            };
        }
    }
}
