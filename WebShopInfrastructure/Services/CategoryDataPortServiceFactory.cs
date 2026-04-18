using WebShopDomain.Model;
using WebShopInfrastructure.Models;

namespace WebShopInfrastructure.Services
{
    public class CategoryDataPortServiceFactory : IDataPortServiceFactory<Category>
    {
        private readonly DbWebShopContext _context;

        public CategoryDataPortServiceFactory(DbWebShopContext context)
        {
            _context = context;
        }

        public IImportService<Category> GetImportService(string contentType)
        {
            return new CategoryImportService(_context);
        }

        public IExportService<Category> GetExportService(string contentType)
        {
            return new CategoryExportService(_context);
        }
    }
}