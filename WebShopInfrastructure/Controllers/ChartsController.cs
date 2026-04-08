using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using WebShopInfrastructure.Models;
using Microsoft.EntityFrameworkCore;

namespace WebShopInfrastructure.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ChartsController : ControllerBase
    {
        private readonly DbWebShopContext dbContext;

        public ChartsController(DbWebShopContext dbContext)
        {
            this.dbContext = dbContext;
        }

        private record CategorySalesItem(string CategoryName, int Count);

        [HttpGet("categorySales")]
        public async Task<JsonResult> GetCategorySales(CancellationToken cancellationToken)
        {
            var rawData = await dbContext.Orderitems
                .Include(x => x.Product)
                    .ThenInclude(p => p.Category)
                .ToListAsync(cancellationToken);

            var result = rawData
                .GroupBy(x => x.Product.Category.Categoryname)
                .Select(g => new CategorySalesItem(
                    g.Key,
                    g.Sum(x => x.Quantity)
                ))
                .OrderByDescending(x => x.Count)
                .ToList();

            return new JsonResult(result);
        }

        private record PopularProductItem(string Name, int Count);

        [HttpGet("popularProducts")]
        public async Task<JsonResult> GetPopularProducts(CancellationToken cancellationToken)
        {
            var rawData = await dbContext.Orderitems
                .Include(x => x.Product)
                .ToListAsync(cancellationToken);

            var result = rawData
                .GroupBy(x => x.Product.Name)
                .Select(g => new PopularProductItem(
                    g.Key,
                    g.Sum(x => x.Quantity)
                ))
                .OrderByDescending(x => x.Count)
                .ToList();

            return new JsonResult(result);
        }
    }
}
