using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebShopDomain.Model;
using WebShopInfrastructure.Models;
using WebShopInfrastructure.Services;

namespace WebShopInfrastructure.Controllers
{
    public class CategoriesController : Controller
    {
        private readonly DbWebShopContext _context;
        private readonly IDataPortServiceFactory<Category> _factory;

        public CategoriesController(
            DbWebShopContext context,
            IDataPortServiceFactory<Category> factory)
        {
            _context = context;
            _factory = factory;
        }

        // GET: Categories
        public async Task<IActionResult> Index()
        {
            return View(await _context.Categories.ToListAsync());
        }

        // GET: Categories/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
                return NotFound();

            var category = await _context.Categories
                .FirstOrDefaultAsync(m => m.Id == id);
            if (category == null)
                return NotFound();

            return RedirectToAction("Index", "Products", new
            {
                id = category.Id,
                name = category.Categoryname
            });
        }

        // GET: Categories/Create
        [Authorize(Roles = "admin")]
        public IActionResult Create()
        {
            return View();
        }

        // POST: Categories/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Categoryname,Categoryinfo,Id")] Category category)
        {
            if (ModelState.IsValid)
            {
                _context.Add(category);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(category);
        }

        // GET: Categories/Edit/5
        [Authorize(Roles = "admin")]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
                return NotFound();

            var category = await _context.Categories.FindAsync(id);
            if (category == null)
                return NotFound();

            return View(category);
        }

        // POST: Categories/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Categoryname,Categoryinfo,Id")] Category category)
        {
            if (id != category.Id)
                return NotFound();

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(category);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CategoryExists(category.Id))
                        return NotFound();
                    else
                        throw;
                }

                return RedirectToAction(nameof(Index));
            }
            return View(category);
        }

        // GET: Categories/Delete/5
        [Authorize(Roles = "admin")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
                return NotFound();

            var category = await _context.Categories
                .FirstOrDefaultAsync(m => m.Id == id);
            if (category == null)
                return NotFound();

            return View(category);
        }

        // POST: Categories/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var category = await _context.Categories.FindAsync(id);

            if (category != null)
                _context.Categories.Remove(category);

            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

        //ІМПОРТ

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
                var importService = _factory.GetImportService(fileExcel.ContentType);

                using var stream = fileExcel.OpenReadStream();
                await importService.ImportFromStreamAsync(stream, cancellationToken);

                TempData["Success"] = "Категорії успішно імпортовано!";
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

        //ЕКСПОРТ

        [Authorize(Roles = "admin")]
        [HttpGet]
        public async Task<IActionResult> Export(
            string contentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
            CancellationToken cancellationToken = default)
        {
            var service = _factory.GetExportService(contentType);

            var stream = new MemoryStream();

            await service.WriteToAsync(stream, cancellationToken);

            stream.Position = 0;

            return File(stream, contentType, $"categories_{DateTime.Now:yyyyMMdd}.xlsx");
        }

        private bool CategoryExists(int id)
        {
            return _context.Categories.Any(e => e.Id == id);
        }
    }
}