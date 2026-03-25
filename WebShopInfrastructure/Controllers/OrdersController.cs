using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebShopInfrastructure.Models;

namespace WebShopInfrastructure.Controllers
{
    public class OrdersController : Controller
    {
        private readonly DbWebShopContext _context;

        public OrdersController(DbWebShopContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            int? customerId = await GetCurrentCustomerIdAsync();

            var allOrders = await _context.Orders.ToListAsync();
            Console.WriteLine($"=== DEBUG ===");
            Console.WriteLine($"Email: {User.Identity?.Name}");
            Console.WriteLine($"CustomerId: {customerId}");
            Console.WriteLine($"Всього замовлень в БД: {allOrders.Count}");
            foreach (var o in allOrders)
                Console.WriteLine($"  Order ID={o.Id}, CustomerID={o.Customerid}, Date={o.Orderdate}");
            Console.WriteLine($"=============");

            if (customerId == null)
                return RedirectToAction("Login", "Account");

            var orders = await _context.Orders
                .Include(o => o.Orderstatus)
                .Where(o => o.Customerid == customerId)
                .OrderByDescending(o => o.Orderdate)
                .ToListAsync();

            return View(orders);
        }

        public async Task<IActionResult> Details(int id)
        {
            int? customerId = await GetCurrentCustomerIdAsync();
            if (customerId == null)
                return RedirectToAction("Login", "Account");

            var order = await _context.Orders
                .Include(o => o.Orderstatus)
                .Include(o => o.Orderitems)
                    .ThenInclude(oi => oi.Product)
                .FirstOrDefaultAsync(o => o.Id == id && o.Customerid == customerId);

            if (order == null)
                return NotFound();

            return View(order);
        }

        private async Task<int?> GetCurrentCustomerIdAsync()
        {
            var email = User.Identity?.Name;

            if (string.IsNullOrEmpty(email))
                return null;

            var customer = await _context.Customers
                .FirstOrDefaultAsync(c => c.Email == email);

            if (customer == null)
                return null;

            return customer.Id;
        }
    }
}