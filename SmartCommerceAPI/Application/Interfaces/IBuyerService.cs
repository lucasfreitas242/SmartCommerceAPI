using SmartCommerceAPI.Models;

namespace SmartCommerceAPI.Application.Interfaces
{
    public interface IBuyerService
    {
        Task<Buyer> GetByIdAsync(string id);
        Task<IEnumerable<Buyer>> GetAllAsync();
        Task CreateAsync(Buyer buyer);
        Task UpdateAsync(string id, Buyer buyer);
        Task DeleteAsync(string id);
    }
}
