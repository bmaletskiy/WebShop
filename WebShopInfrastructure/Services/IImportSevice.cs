namespace WebShopInfrastructure.Services
{
    public interface IImportSevice<TEntity>
        where TEntity : class
    {
        Task ImportFromStreamAsync(Stream stream, CancellationToken cancellationToken);
    }
}
