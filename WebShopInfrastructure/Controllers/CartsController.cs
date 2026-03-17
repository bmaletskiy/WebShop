using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebShopDomain.Model;
using WebShopInfrastructure.Models;

namespace WebShopInfrastructure.Controllers
{
    [Authorize]
    public class CartsController : Controller
    {
        private readonly DbWebShopContext _context;

        public CartsController(DbWebShopContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            int? customerId = await GetCurrentCustomerIdAsync();
            if (customerId == null)
                return RedirectToAction("Login", "Account");

            var cart = await _context.Carts
                .Include(c => c.Cartitems)
                .ThenInclude(ci => ci.Product)
                .FirstOrDefaultAsync(c => c.Customerid == customerId);

            if (cart == null)
            {
                cart = new Cart
                {
                    Customerid = customerId,
                    Createdat = DateTime.Now,
                    Updatedat = DateTime.Now
                };

                _context.Carts.Add(cart);
                await _context.SaveChangesAsync();
            }

            return View(cart);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddToCart(int productId)
        {
            int? customerId = await GetCurrentCustomerIdAsync();
            if (customerId == null)
                return RedirectToAction("Login", "Account");

            var product = await _context.Products.FirstOrDefaultAsync(p => p.Id == productId);
            if (product == null)
                return NotFound();

            var cart = await _context.Carts
                .Include(c => c.Cartitems)
                .FirstOrDefaultAsync(c => c.Customerid == customerId);

            if (cart == null)
            {
                cart = new Cart
                {
                    Customerid = customerId,
                    Createdat = DateTime.Now,
                    Updatedat = DateTime.Now
                };

                _context.Carts.Add(cart);
                await _context.SaveChangesAsync();
            }

            var existingItem = cart.Cartitems.FirstOrDefault(ci => ci.Productid == productId);

            if (existingItem == null)
            {
                var cartItem = new Cartitem
                {
                    Cartid = cart.Id,
                    Productid = productId,
                    Quantity = 1
                };

                _context.Cartitems.Add(cartItem);
            }
            else
            {
                existingItem.Quantity = (existingItem.Quantity ?? 0) + 1;
            }

            cart.Updatedat = DateTime.Now;

            await _context.SaveChangesAsync();
            return RedirectToAction("Index");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Remove(int productId)
        {
            int? customerId = await GetCurrentCustomerIdAsync();
            if (customerId == null)
                return RedirectToAction("Login", "Account");

            var cart = await _context.Carts
                .Include(c => c.Cartitems)
                .FirstOrDefaultAsync(c => c.Customerid == customerId);

            if (cart == null)
                return RedirectToAction(nameof(Index));

            var cartItem = cart.Cartitems.FirstOrDefault(ci => ci.Productid == productId);

            if (cartItem != null)
            {
                _context.Cartitems.Remove(cartItem);
                cart.Updatedat = DateTime.Now;
                await _context.SaveChangesAsync();
            }

            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Clear()
        {
            int? customerId = await GetCurrentCustomerIdAsync();
            if (customerId == null)
                return RedirectToAction("Login", "Account");

            var cart = await _context.Carts
                .Include(c => c.Cartitems)
                .FirstOrDefaultAsync(c => c.Customerid == customerId);

            if (cart != null && cart.Cartitems.Any())
            {
                _context.Cartitems.RemoveRange(cart.Cartitems);
                cart.Updatedat = DateTime.Now;
                await _context.SaveChangesAsync();
            }

            return RedirectToAction(nameof(Index));
        }

        private async Task<int?> GetCurrentCustomerIdAsync()
        {
            var email = User.Identity?.Name;

            if (string.IsNullOrEmpty(email))
                return null;

            var customer = await _context.Customers
                .FirstOrDefaultAsync(c => c.Email == email);

            if (customer == null)
            {
                customer = new Customer
                {
                    Email = email,
                    Fullname = email
                };

                _context.Customers.Add(customer);
                await _context.SaveChangesAsync();
            }

            return customer.Id;
        }
    }
}