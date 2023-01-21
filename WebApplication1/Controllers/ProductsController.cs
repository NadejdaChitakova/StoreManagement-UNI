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
            var products = new List<Product>();
            List<SelectListItem> selectListItems = new List<SelectListItem>()
            {
                new SelectListItem { Value = "1", Text = "all" },
                new SelectListItem { Value = "CBFB62EF-181A-497E-A7D7-3262DC68EBE3", Text = "building materials" },
                new SelectListItem { Value = "C96D7B4F-D7C1-4C54-AD20-3866764F9758", Text = "groceries" },
                new SelectListItem { Value = "CC67C818-2456-466B-96DB-6A2BAE530DE3", Text = "stationery" },
            };
            ViewBag.Categories = selectListItems;

            products = _product.GetProducts();

            return View(products);
        }

        [HttpPost]
        public async Task<IActionResult> Index(string Categories, int uniqueCode)
        {
            var products = new List<Product>();
            List<SelectListItem> selectListItems = new List<SelectListItem>()
            {
                new SelectListItem { Value = "1", Text = "all" },
                new SelectListItem { Value = "CBFB62EF-181A-497E-A7D7-3262DC68EBE3", Text = "building materials" },
                new SelectListItem { Value = "C96D7B4F-D7C1-4C54-AD20-3866764F9758", Text = "groceries" },
                new SelectListItem { Value = "CC67C818-2456-466B-96DB-6A2BAE530DE3", Text = "stationery" },
            };
            ViewBag.Categories = selectListItems;

            products = _product.GetProductByCategory(Categories);

            if (uniqueCode != null)
            {
                products = products.Where(x => x.ProductCode == uniqueCode).ToList();
            }

            return View(products);
        }

        public async Task<IActionResult> Details(Guid? id)
        {
            if (id == null || _context.Product == null)
            {
                return NotFound();
            }

            var product = await _context.Product
                .FirstOrDefaultAsync(m => m.Id == id);
            if (product == null)
            {
                return NotFound();
            }

            return View(product);
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

        public async Task<IActionResult> Edit(Guid id)
        {
            var product = _product.FindProductById(id);

            if (id == null || product == null || product.Id == null)
            {
                return NotFound();
            }

            return View(MapEntityToVM(product));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id, ProductVM product)
        {
            if (id != product.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _product.UpdateProduct(MapVmToDTO(product));
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ProductExists(product.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(product);
        }

        // GET: Products/Delete/5
        public async Task<IActionResult> Delete(Guid? id)
        {
            if (id == null || _context.Product == null)
            {
                return NotFound();
            }

            var product = await _context.Product
                .FirstOrDefaultAsync(m => m.Id == id);
            if (product == null)
            {
                return NotFound();
            }

            return View(product);
        }

        // POST: Products/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            if (_context.Product == null)
            {
                return Problem("Entity set 'ApplicationDBContext.Product'  is null.");
            }
            var product = await _context.Product.FindAsync(id);
            if (product != null)
            {
                _context.Product.Remove(product);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ProductExists(Guid id)
        {
            return _context.Product.Any(e => e.Id == id);
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

        public IActionResult Create(string id = null)
        {
            List<Category> categories = _context.Category.ToList();
            if (id != null)
            {
                ViewBag.ListOfCategories = new SelectList(categories, "Id", "Name");
            }
            return View();
        }
    }
}
