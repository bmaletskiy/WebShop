using WebShopDomain.Model;
using WebShopInfrastructure.Models;

namespace WebShopInfrastructure.Services
{
    public class ProductDataPortServiceFactory : IDataPortServiceFactory<Product>
    {
        private readonly DbWebShopContext _context;

        public ProductDataPortServiceFactory(DbWebShopContext context)
        {
            _context = context;
        }

        public IImportService<Product> GetImportService(string contentType)
        {
            if (contentType == "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet")
            {
                return new ProductImportService(_context);
            }

            throw new NotImplementedException();
        }

        public IExportService<Product> GetExportService(string contentType)
        {
            if (contentType == "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet")
            {
                return new ProductExportService(_context);
            }

            throw new NotImplementedException();
        }
    }
}