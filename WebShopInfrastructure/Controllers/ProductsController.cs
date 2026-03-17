using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
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

        public async Task<IActionResult> Index()
        {
            var products = await _context.Products
                .Include(p => p.Category)
                .ToListAsync();

            return View(products);
        }
    }
}