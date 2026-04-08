using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebShopDomain.Model;
using WebShopInfrastructure.Models;

namespace WebShopInfrastructure.Controllers
{
    public class ProductsController : Controller
    {
        private readonly DbWebShopContext _context;

        public ProductsController(DbWebShopContext context)
        {
            _context = context;
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

        private bool ProductExists(int id)
        {
            return _context.Products.Any(e => e.Id == id);
        }
    }
}