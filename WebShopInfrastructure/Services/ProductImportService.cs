using ClosedXML.Excel;
using Microsoft.EntityFrameworkCore;
using WebShopDomain.Model;
using WebShopInfrastructure.Models;

namespace WebShopInfrastructure.Services
{
    public class ProductImportService : IImportService<Product>
    {
        private readonly DbWebShopContext _context;

        public ProductImportService(DbWebShopContext context)
        {
            _context = context;
        }

        public async Task ImportFromStreamAsync(Stream stream, CancellationToken cancellationToken)
        {
            if (!stream.CanRead)
                throw new ArgumentException("Дані не можуть бути прочитані", nameof(stream));

            using (var workbook = new XLWorkbook(stream))
            {
                foreach (var worksheet in workbook.Worksheets)
                {
                    var categoryName = worksheet.Name;

                    var category = await _context.Categories
                        .FirstOrDefaultAsync(c => c.Categoryname == categoryName, cancellationToken);

                    if (category == null)
                    {
                        category = new Category
                        {
                            Categoryname = categoryName
                        };

                        _context.Categories.Add(category);
                        await _context.SaveChangesAsync(cancellationToken); 
                    }

                    foreach (var row in worksheet.RowsUsed().Skip(1))
                    {
                        await AddOrUpdateProductAsync(row, category, cancellationToken);
                    }
                }
            }

            await _context.SaveChangesAsync(cancellationToken);
        }

        private async Task AddOrUpdateProductAsync(IXLRow row, Category category, CancellationToken cancellationToken)
        {
            var productName = row.Cell(1).GetValue<string>();

            if (string.IsNullOrWhiteSpace(productName))
                return;

            decimal price;
            int quantity;

            //ВАЛІДАЦІЯ
            if (!decimal.TryParse(row.Cell(3).GetValue<string>(), out price))
                return;

            if (!int.TryParse(row.Cell(4).GetValue<string>(), out quantity))
                quantity = 0;

            var existingProduct = await _context.Products
                .FirstOrDefaultAsync(p => p.Name == productName && p.Categoryid == category.Id, cancellationToken);

            if (existingProduct != null)
            {
                existingProduct.Description = row.Cell(2).GetValue<string>();
                existingProduct.Price = price;
                existingProduct.Availableqty = quantity;
                existingProduct.Updatedat = DateTime.Now;

                return;
            }

            Product product = new Product
            {
                Name = productName,
                Description = row.Cell(2).GetValue<string>(),
                Price = price,
                Availableqty = quantity,
                Categoryid = category.Id,
                Createdat = DateTime.Now,
                Updatedat = DateTime.Now
            };

            _context.Products.Add(product);
        }
    }
}