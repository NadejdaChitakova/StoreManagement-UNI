using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System.Collections;
using System.Drawing;
using System.Drawing.Imaging;
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

        // GET: Products
        public async Task<IActionResult> Index()
        {
            return View(await _context.Product.ToListAsync());
        }

        // GET: Products/Details/5
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

        // GET: Products/Create
        public IActionResult Create()
        {
            List<Category> categories = _context.Category.ToList();
            ViewBag.ListOfCategories = new SelectList(categories, "Id", "Name");
            return View();
        }

        // POST: Products/Create

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

        // GET: Products/Edit/5
        public async Task<IActionResult> Edit(Guid id)
        {
            var product = _product.FindProductById(id);

            if (id == null || product == null || product.Id == null)
            {
                return NotFound();
            }

            return View(MapEntityToVM(product));
        }

        // POST: Products/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id, [Bind("Id,Name,Description,BuyPrice,SellPrice,Picture,CategoryId")] Product product)
        {
            if (id != product.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(product);
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
            //var picture = _product.ReadFileFromDB(dto.PictureName, dto.Picture, dto.PictureFormat);
            //var img = _product.ConvertImageFromIForm(dto.Picture);
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
        //public ActionResult GetImg(ProductDTO dto)
        //{
        //    using (var ms = new MemoryStream(dto.Picture))
        //    {
        //        return Image.FromStream(ms);
        //    }
        //}
    }
}
