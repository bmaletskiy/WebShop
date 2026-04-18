using ClosedXML.Excel;
using Microsoft.EntityFrameworkCore;
using WebShopDomain.Model;
using WebShopInfrastructure.Models;

namespace WebShopInfrastructure.Services
{
    public class CategoryExportService : IExportService<Category>
    {
        private readonly DbWebShopContext _context;

        public CategoryExportService(DbWebShopContext context)
        {
            _context = context;
        }

        public async Task WriteToAsync(Stream stream, CancellationToken cancellationToken)
        {
            using var workbook = new XLWorkbook();
            var worksheet = workbook.Worksheets.Add("Категорії");

            worksheet.Cell(1, 1).Value = "Назва категорії";
            worksheet.Cell(1, 2).Value = "Інформація";

            var categories = await _context.Categories
                .AsNoTracking()
                .ToListAsync(cancellationToken);

            int row = 2;

            foreach (var category in categories)
            {
                worksheet.Cell(row, 1).Value = category.Categoryname;
                worksheet.Cell(row, 2).Value = category.Categoryinfo;

                row++;
            }

            worksheet.Columns().AdjustToContents();

            workbook.SaveAs(stream);
        }
    }
}