using SmartCommerceAPI.Models;

namespace SmartCommerceAPI.Application.Interfaces
{
    public interface IBuyerService
    {
        Task<List<Buyer>> GetBuyersAsync();
        Task<List<Buyer>> FilterBuyersAsync(BuyerFilter filter);
        Task<Buyer> CreateBuyerAsync(Buyer buyer);
        Task<bool> UpdateBuyerAsync(Guid id, Buyer buyerIn);
        Task<object> ValidateFieldsAsync(Buyer buyer);
    }
}
