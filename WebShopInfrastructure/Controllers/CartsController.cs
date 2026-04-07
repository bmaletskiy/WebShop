using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebShopDomain.Model;
using WebShopInfrastructure.Models;

namespace WebShopInfrastructure.Controllers
{
    public class CartsController : Controller
    {
        private readonly DbWebShopContext _context;

        public CartsController(DbWebShopContext context)
        {
            _context = context;
        }

        [Authorize(Roles = "user, admin")]
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

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Checkout()
        {
            try
            {
                int? customerId = await GetCurrentCustomerIdAsync();
                if (customerId == null)
                    return RedirectToAction("Login", "Account");

                var cart = await _context.Carts
                    .Include(c => c.Cartitems)
                    .ThenInclude(ci => ci.Product)
                    .FirstOrDefaultAsync(c => c.Customerid == customerId);

                if (cart == null) return Content("ПОМИЛКА: Cart not found");
                if (!cart.Cartitems.Any()) return Content("ПОМИЛКА: Cart is empty");

                var total = cart.Cartitems.Sum(ci => (ci.Product.Price ?? 0) * (ci.Quantity ?? 0));

                var order = new Order
                {
                    Customerid = customerId,
                    Orderstatusid = 1,
                    Orderdate = DateTime.Now,
                    Totalamount = total
                };

                _context.Orders.Add(order);
                await _context.SaveChangesAsync();

                foreach (var item in cart.Cartitems)
                {
                    var orderItem = new Orderitem
                    {
                        Orderid = order.Id,
                        Productid = item.Productid,
                        Quantity = item.Quantity ?? 0,
                        Unitprice = item.Product?.Price ?? 0,
                        Totalprice = (item.Product?.Price ?? 0) * (item.Quantity ?? 0)
                    };
                    _context.Orderitems.Add(orderItem);
                }

                _context.Cartitems.RemoveRange(cart.Cartitems);
                await _context.SaveChangesAsync();

                return RedirectToAction("Index", "Orders");
            }
            catch (Exception ex)
            {
                return Content($"ПОМИЛКА: {ex.Message} | Inner: {ex.InnerException?.Message}");
            }
        }
    }
}