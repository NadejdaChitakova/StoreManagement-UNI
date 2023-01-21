using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using WebApplication1.Data;
using WebApplication1.Models;
using WebApplication1.Models.Domain;
using WebApplication1.Models.Entity;
using WebApplication1.Services.Interfaces;

namespace WebApplication1.Controllers
{
    public class ProductsController : Controller
    {
        private readonly ApplicationDBContext _context;
        private readonly IProduct _product;

        public ProductsController(ApplicationDBContext context, IProduct product)
        {
            _context = context;
            _product = product;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            List<SelectListItem> selectListItems = new List<SelectListItem>()
            {
                new SelectListItem { Value = "1", Text = "all" },
                new SelectListItem { Value = "CBFB62EF-181A-497E-A7D7-3262DC68EBE3", Text = "building materials" },
                new SelectListItem { Value = "C96D7B4F-D7C1-4C54-AD20-3866764F9758", Text = "groceries" },
                new SelectListItem { Value = "CC67C818-2456-466B-96DB-6A2BAE530DE3", Text = "stationery" },
            };
            ViewBag.Categories = selectListItems;

            var products = _product.GetProducts().ToList();
            var productVms = new List<ProductVM>();
            foreach (var item in products)
            {
                productVms.Add(MapEntityToVM(item));
            }

            return View(productVms);
        }

        [HttpPost]
        public async Task<IActionResult> Index(string Categories, int uniqueCode)
        {
            var products = _product.GetProductByCategory(Categories).ToList();
            List<SelectListItem> selectListItems = new List<SelectListItem>()
            {
                new SelectListItem { Value = "1", Text = "all" },
                new SelectListItem { Value = "CBFB62EF-181A-497E-A7D7-3262DC68EBE3", Text = "building materials" },
                new SelectListItem { Value = "C96D7B4F-D7C1-4C54-AD20-3866764F9758", Text = "groceries" },
                new SelectListItem { Value = "CC67C818-2456-466B-96DB-6A2BAE530DE3", Text = "stationery" },
            };
            ViewBag.Categories = selectListItems;

            var productVms = new List<ProductVM>();
            foreach (var item in products)
            {
                productVms.Add(MapEntityToVM(item));
            }

            if (uniqueCode != null)
            {
                productVms = productVms.Where(x => x.ProductCode == uniqueCode).ToList();
            }

            return View(productVms);
        }

        public async Task<IActionResult> Details(Guid id)
        {
            if (id == null || _context.Product == null)
            {
                return NotFound();
            }

            var product = _product.FindProductById(id);
            var productVM = MapEntityToVM(product);
            productVM.CategoryName = _context.Category.Where(x => x.Id.ToString() == product.CategoryId).Select(x => x.Name).FirstOrDefault();
            if (product == null)
            {
                return NotFound();
            }

            return View(productVM);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(ProductVM product)
        {
            if (ModelState.IsValid)
            {
                ProductDTO productDTO = MapVmToDTO(product);
                var statusCodeResult = await _product.CreateProduct(productDTO);

                if (statusCodeResult.StatusCode == StatusCodes.Status201Created)
                {
                    return RedirectToAction(nameof(Index));
                }//TODO : else return error code
            }
            return View(product);
        }

        [HttpGet]
        public async Task<IActionResult> Edit(Guid id)
        {
            List<SelectListItem> selectListItems = new List<SelectListItem>()
            {
                new SelectListItem { Value = "CBFB62EF-181A-497E-A7D7-3262DC68EBE3", Text = "building materials" },
                new SelectListItem { Value = "C96D7B4F-D7C1-4C54-AD20-3866764F9758", Text = "groceries" },
                new SelectListItem { Value = "CC67C818-2456-466B-96DB-6A2BAE530DE3", Text = "stationery" },
            };
            ViewBag.Categories = selectListItems;
            var product = _product.FindProductById(id);

            if (id == null || product == null || product.Id == null)
            {
                return NotFound();
            }

            return View(MapEntityToVM(product));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id, ProductVM product, string Categories)
        {
            List<SelectListItem> selectListItems = new List<SelectListItem>()
            {
                new SelectListItem { Value = "CBFB62EF-181A-497E-A7D7-3262DC68EBE3", Text = "building materials" },
                new SelectListItem { Value = "C96D7B4F-D7C1-4C54-AD20-3866764F9758", Text = "groceries" },
                new SelectListItem { Value = "CC67C818-2456-466B-96DB-6A2BAE530DE3", Text = "stationery" },
            };
            ViewBag.Categories = selectListItems;
            if (id != product.Id)
            {
                return NotFound();
            }

            _product.UpdateProduct(MapVmToDTOEditForm(product, id.ToString()));

            return RedirectToAction(nameof(Index));

        }

        // GET: Products/Delete/5
        public async Task<IActionResult> Delete(Guid id)
        {
            if (id == null || _context.Product == null)
            {
                return NotFound();
            }

            _product.DeleteProduct(id);

            return RedirectToAction(nameof(Index));
        }

        private ProductDTO MapVmToDTO(ProductVM productVM)
        {
            var memoryStream = new MemoryStream();
            productVM.Picture.CopyTo(memoryStream);

            return new ProductDTO()
            {
                Id = productVM.Id,
                Name = productVM.Name,
                Description = productVM.Description,
                BuyPrice = productVM.BuyPrice,
                SellPrice = productVM.SellPrice,
                CategoryId = productVM.CategoryId,
                PictureName = productVM.Picture.FileName,
                Picture = memoryStream.ToArray(),
                PictureFormat = productVM.Picture.ContentType,
                ProductCode = productVM.ProductCode,
                ProductCount = productVM.ProductCount
            };
        }
        private ProductDTO MapVmToDTOEditForm(ProductVM productVM, string id)
        {
            return new ProductDTO()
            {
                Id = productVM.Id,
                Name = productVM.Name,
                Description = productVM.Description,
                BuyPrice = productVM.BuyPrice,
                SellPrice = productVM.SellPrice,
                CategoryId = id,
                ProductCode = productVM.ProductCode,
                ProductCount = productVM.ProductCount
            };
        }
        private ProductVM MapEntityToVM(ProductDTO dto)
        {
            var product = new ProductVM()
            {
                Id = dto.Id,
                Name = dto.Name,
                Description = dto.Description,
                BuyPrice = dto.BuyPrice,
                SellPrice = dto.SellPrice,
                CategoryId = dto.CategoryId,
                ProductCode = dto.ProductCode,
                ProductCount = dto.ProductCount,
                ImageImage = "data:image/jpeg;base64," + Convert.ToBase64String(dto.Picture)
            };

            return product;
        }
    }
}
