using ClosedXML.Excel;
using Microsoft.EntityFrameworkCore;
using WebShopDomain.Model;
using WebShopInfrastructure.Models;

namespace WebShopInfrastructure.Services
{
    public class CategoryImportService : IImportService<Category>
    {
        private readonly DbWebShopContext _context;

        public CategoryImportService(DbWebShopContext context)
        {
            _context = context;
        }

        public async Task ImportFromStreamAsync(Stream stream, CancellationToken cancellationToken)
        {
            if (!stream.CanRead)
                throw new ArgumentException("Дані не можуть бути прочитані");

            int importedCount = 0;

            using var workbook = new XLWorkbook(stream);
            var worksheet = workbook.Worksheets.First();

            foreach (var row in worksheet.RowsUsed().Skip(1))
            {
                if (await AddOrUpdateCategoryAsync(row, cancellationToken))
                {
                    importedCount++;
                }
            }

            if (importedCount == 0)
            {
                throw new Exception("Файл не містить коректних даних або має неправильну структуру");
            }

            await _context.SaveChangesAsync(cancellationToken);
        }

        private async Task<bool> AddOrUpdateCategoryAsync(IXLRow row, CancellationToken cancellationToken)
        {
            var categoryName = row.Cell(1).GetValue<string>();
            var categoryInfo = row.Cell(2).GetValue<string>();

            if (string.IsNullOrWhiteSpace(categoryName))
                return false;

            var existingCategory = await _context.Categories
                .FirstOrDefaultAsync(c => c.Categoryname == categoryName, cancellationToken);

            if (existingCategory == null)
            {
                _context.Categories.Add(new Category
                {
                    Categoryname = categoryName,
                    Categoryinfo = categoryInfo
                });

                return true;
            }

            existingCategory.Categoryinfo = categoryInfo;
            return true;
        }
    }
}