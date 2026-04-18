using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebShopDomain.Model;
using WebShopInfrastructure.Models;
using WebShopInfrastructure.Services;

namespace WebShopInfrastructure.Controllers
{
    public class ProductsController : Controller
    {
        private readonly DbWebShopContext _context;
        private readonly IDataPortServiceFactory<Product> _dataPortFactory;

        public ProductsController(
            DbWebShopContext context,
            IDataPortServiceFactory<Product> dataPortFactory)
        {
            _context = context;
            _dataPortFactory = dataPortFactory;
        }

        public async Task<IActionResult> Index(int? id)
        {
            var products = _context.Products.Include(p => p.Category).AsQueryable();

            if (id != null)
                products = products.Where(p => p.Categoryid == id);

            if (id != null)
            {
                var categoryName = await _context.Categories
                    .Where(c => c.Id == id)
                    .Select(c => c.Categoryname)
                    .FirstOrDefaultAsync();

                ViewBag.CategoryName = categoryName;
            }
            else
            {
                ViewBag.CategoryName = "Всі";
            }

            ViewBag.CategoryId = id;

            return View(await products.ToListAsync());
        }

        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
                return NotFound();

            var product = await _context.Products
                .Include(p => p.Category)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (product == null)
                return NotFound();

            return View(product);
        }

        [Authorize(Roles = "admin")]
        public async Task<IActionResult> Create(int? categoryId)
        {
            ViewBag.CategoryId = categoryId;

            if (categoryId != null)
            {
                var categoryName = await _context.Categories
                    .Where(c => c.Id == categoryId)
                    .Select(c => c.Categoryname)
                    .FirstOrDefaultAsync();

                ViewBag.CategoryName = categoryName;
            }

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Product product)
        {
            if (ModelState.IsValid)
            {
                product.Createdat = DateTime.Now;
                product.Updatedat = DateTime.Now;

                _context.Add(product);
                await _context.SaveChangesAsync();

                return RedirectToAction(nameof(Index), new
                {
                    id = product.Categoryid
                });
            }

            return View(product);
        }

        [Authorize(Roles = "admin")]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
                return NotFound();

            var product = await _context.Products.FindAsync(id);
            if (product == null)
                return NotFound();

            return View(product);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Product product)
        {
            if (id != product.Id)
                return NotFound();

            if (ModelState.IsValid)
            {
                try
                {
                    product.Updatedat = DateTime.Now;
                    _context.Update(product);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ProductExists(product.Id))
                        return NotFound();
                    else
                        throw;
                }

                return RedirectToAction(nameof(Index), new
                {
                    id = product.Categoryid
                });
            }

            return View(product);
        }

        [Authorize(Roles = "admin")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
                return NotFound();

            var product = await _context.Products
                .Include(p => p.Category)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (product == null)
                return NotFound();

            return View(product);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var product = await _context.Products.FindAsync(id);

            if (product != null)
            {
                var categoryId = product.Categoryid;

                _context.Products.Remove(product);
                await _context.SaveChangesAsync();

                return RedirectToAction(nameof(Index), new
                {
                    id = categoryId
                });
            }

            return RedirectToAction(nameof(Index));
        }

        [Authorize(Roles = "admin")]
        [HttpGet]
        public IActionResult Import()
        {
            return View();
        }

        [Authorize(Roles = "admin")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Import(IFormFile fileExcel, CancellationToken cancellationToken)
        {
            if (fileExcel == null || fileExcel.Length == 0)
            {
                ModelState.AddModelError("", "Оберіть файл");
                return View();
            }

            var extension = Path.GetExtension(fileExcel.FileName);
            if (extension != ".xlsx")
            {
                ModelState.AddModelError("", "Дозволені тільки файли .xlsx");
                return View();
            }

            try
            {
                var importService = _dataPortFactory.GetImportService(fileExcel.ContentType);

                using var stream = fileExcel.OpenReadStream();
                await importService.ImportFromStreamAsync(stream, cancellationToken);

                TempData["Success"] = "Файл успішно імпортовано!";
                return RedirectToAction(nameof(Index));
            }
            catch (ArgumentException ex)
            {
                ModelState.AddModelError("", ex.Message);
            }
            catch (Exception)
            {
                ModelState.AddModelError("", "Помилка при імпорті файлу");
            }

            return View();
        }

        [Authorize(Roles = "admin")]
        [HttpGet]
        public async Task<IActionResult> Export(
            string contentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
            CancellationToken cancellationToken = default)
        {
            var exportService = _dataPortFactory.GetExportService(contentType);

            var stream = new MemoryStream();

            await exportService.WriteToAsync(stream, cancellationToken);

            stream.Position = 0;

            return File(stream, contentType, $"products_{DateTime.Now:yyyyMMdd}.xlsx");
        }

        private bool ProductExists(int id)
        {
            return _context.Products.Any(e => e.Id == id);
        }
    }
}