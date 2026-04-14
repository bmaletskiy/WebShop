using ClosedXML.Excel;
using Microsoft.EntityFrameworkCore;
using WebShopDomain.Model;
using WebShopInfrastructure.Models;

namespace WebShopInfrastructure.Services
{
    public class ProductExportService : IExportService<Product>
    {
        private readonly DbWebShopContext _context;

        public ProductExportService(DbWebShopContext context)
        {
            _context = context;
        }

        public async Task WriteToAsync(Stream stream, CancellationToken cancellationToken)
        {
            var workbook = new XLWorkbook();

            var categories = await _context.Categories
                .Include(c => c.Products)
                .ToListAsync(cancellationToken);

            foreach (var category in categories)
            {
                var worksheet = workbook.Worksheets.Add(category.Categoryname);

                worksheet.Cell(1, 1).Value = "Name";
                worksheet.Cell(1, 2).Value = "Description";
                worksheet.Cell(1, 3).Value = "Price";

                int row = 2;

                foreach (var product in category.Products)
                {
                    worksheet.Cell(row, 1).Value = product.Name;
                    worksheet.Cell(row, 2).Value = product.Description;
                    worksheet.Cell(row, 3).Value = product.Price;

                    row++;
                }
            }

            workbook.SaveAs(stream);
        }
    }
}