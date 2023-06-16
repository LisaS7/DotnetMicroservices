using Play.Catalogue.Service.Entities;

namespace Play.Catalogue.Service.Repositories
{
    public interface IRepository<T> where T : IEntity
    {
        Task CreateAsync(T entity);
        Task<IReadOnlyCollection<T>> GetAllAsync();
        Task<T> GetAsync(Guid id);
        Task DeleteAsync(Guid id);
        Task UpdateAsync(T entity);
    }
}