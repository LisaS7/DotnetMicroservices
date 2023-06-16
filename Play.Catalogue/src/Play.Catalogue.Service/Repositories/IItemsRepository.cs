using Play.Catalogue.Service.Entities;

namespace Play.Catalogue.Service.Repositories
{
    public interface IItemsRepository
    {
        Task CreateAsync(Item entity);
        Task<IReadOnlyCollection<Item>> GetAllAsync();
        Task<Item> GetAsync(Guid id);
        Task DeleteAsync(Guid id);
        Task UpdateAsync(Item entity);
    }
}