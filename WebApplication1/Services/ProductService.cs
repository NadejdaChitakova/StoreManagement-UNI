using Microsoft.AspNetCore.Mvc;
using System.Drawing;
using WebApplication1.Data;
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

        public List<ProductDTO> GetProducts()
        {
            var products = _applicationDBContext.Product.ToList();
            var productDTOs = new List<ProductDTO>();
            foreach (var product in products)
            {
                productDTOs.Add(MapEntityToDTO(product));
            }
            return productDTOs;
        }

        public ProductDTO FindProductById(Guid productId)
        {
            var product = _applicationDBContext.Product.Where(x => x.Id == productId).SingleOrDefault();
            return MapEntityToDTO(product);
        }
        public List<ProductDTO> GetProductByCategory(string categoryId)
        {
            if (categoryId.ToUpper().Equals("ALL"))
            {
                var product = _applicationDBContext.Product.ToList();
                var productDTO = new List<ProductDTO>();
                foreach (var item in product)
                {
                    MapEntityToDTO(item);
                }
                return productDTO;
            }

            var products = _applicationDBContext.Product.Where(x => x.CategoryId == categoryId.ToUpper()).ToList();
            var productDTOs = new List<ProductDTO>();

            foreach (var item in products)
            {
                MapEntityToDTO(item);
            }

            return productDTOs;
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

        public void DeleteProduct(Guid productId)
        {
            var product = _applicationDBContext.Product.Where(x => x.Id == productId).FirstOrDefault();
            _applicationDBContext.Product.Remove(product);
            _applicationDBContext.SaveChanges();
        }

        public void UpdateProduct(ProductDTO productDTO)
        {
            var oldProduct = _applicationDBContext.Product.Where(x => x.Id == productDTO.Id).SingleOrDefault();
            var categoryId = _applicationDBContext.Category.Where(x => x.Id.ToString() == productDTO.CategoryId).Select(x => x.Id).FirstOrDefault();
            oldProduct.Name = productDTO.Name;
            oldProduct.Description = productDTO.Description;
            oldProduct.SellPrice = productDTO.SellPrice;
            oldProduct.BuyPrice = productDTO.BuyPrice;
            oldProduct.ProductCode = productDTO.ProductCode;
            oldProduct.CategoryId = categoryId.ToString();
            oldProduct.ProductCount = productDTO.ProductCount;
            _applicationDBContext.Product.Update(oldProduct);
            _applicationDBContext.SaveChanges();
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
                Picture = dto.Picture,
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
