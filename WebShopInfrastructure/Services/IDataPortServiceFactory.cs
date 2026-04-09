namespace WebShopInfrastructure.Services
{
    public interface IDataPortServiceFactory<TEntity>
        where TEntity : class
    {
        IImportSevice<TEntity> GetImportService(string contentType);
        IExportService<TEntity> GetExportService(string contentType);
    }
}
