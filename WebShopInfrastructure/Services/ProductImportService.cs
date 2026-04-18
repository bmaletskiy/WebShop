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

            var errors = new List<string>();

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
                        await AddOrUpdateProductAsync(row, category, errors, cancellationToken);
                    }
                }
            }

            if (errors.Any())
            {
                throw new ArgumentException(string.Join("\n", errors));
            }

            await _context.SaveChangesAsync(cancellationToken);
        }

        private async Task AddOrUpdateProductAsync(
            IXLRow row,
            Category category,
            List<string> errors,
            CancellationToken cancellationToken)
        {
            var rowNumber = row.RowNumber();

            var productName = row.Cell(1).GetValue<string>();

            if (string.IsNullOrWhiteSpace(productName))
            {
                errors.Add($"Рядок {rowNumber}: порожня назва товару");
                return;
            }

            if (!decimal.TryParse(row.Cell(3).GetString(), out decimal price))
            {
                errors.Add($"Рядок {rowNumber}: некоректна ціна");
                return;
            }

            if (!int.TryParse(row.Cell(4).GetString(), out int quantity))
            {
                errors.Add($"Рядок {rowNumber}: некоректна кількість");
                return;
            }

            if (quantity < 0)
            {
                errors.Add($"Рядок {rowNumber}: кількість не може бути від’ємною");
                return;
            }

            var existingProduct = await _context.Products
                .FirstOrDefaultAsync(p => p.Name == productName && p.Categoryid == category.Id, cancellationToken);

            if (existingProduct != null)
            {
                existingProduct.Description = row.Cell(2).GetValue<string>();
                existingProduct.Price = price;
                existingProduct.Availableqty = quantity;
                existingProduct.Updatedat = DateTime.Now;
            }
            else
            {
                _context.Products.Add(new Product
                {
                    Name = productName,
                    Description = row.Cell(2).GetValue<string>(),
                    Price = price,
                    Availableqty = quantity,
                    Categoryid = category.Id,
                    Createdat = DateTime.Now,
                    Updatedat = DateTime.Now
                });
            }
        }
    }
}