using MongoDB.Driver;
using SmartCommerceAPI.Application.Interfaces;
using SmartCommerceAPI.Data;
using SmartCommerceAPI.Models;

namespace SmartCommerceAPI.Application.Services
{
    public class BuyerService : IBuyerService
    {
        private readonly IMongoCollection<Buyer> _buyers;

        public BuyerService(MongoDbContext context)
        {
            _buyers = context.Buyers;
        }

        public async Task<List<Buyer>> GetBuyersAsync()
        {
            return await _buyers.Find(FilterDefinition<Buyer>.Empty)
                                 .Limit(20)
                                 .ToListAsync();
        }

        public async Task<List<Buyer>> FilterBuyersAsync(BuyerFilter filter)
        {
            var filterDefinition = Builders<Buyer>.Filter.Empty;

            if (!string.IsNullOrEmpty(filter.Name))
                filterDefinition &= Builders<Buyer>.Filter.Regex("Name", new MongoDB.Bson.BsonRegularExpression(filter.Name, "i"));
            if (!string.IsNullOrEmpty(filter.Email))
                filterDefinition &= Builders<Buyer>.Filter.Eq("Email", filter.Email);
            if (!string.IsNullOrEmpty(filter.Phone))
                filterDefinition &= Builders<Buyer>.Filter.Eq("Phone", filter.Phone);
            if (!string.IsNullOrEmpty(filter.PersonType))
                filterDefinition &= Builders<Buyer>.Filter.Eq("PersonType", filter.PersonType);
            if (!string.IsNullOrEmpty(filter.Document))
                filterDefinition &= Builders<Buyer>.Filter.Eq("CpfCnpj", filter.Document);
            if (!string.IsNullOrEmpty(filter.StateRegistration))
                filterDefinition &= Builders<Buyer>.Filter.Eq("StateRegistration", filter.StateRegistration);
            if (filter.Blocked.HasValue)
                filterDefinition &= Builders<Buyer>.Filter.Eq("Blocked", filter.Blocked.Value);

            return await _buyers.Find(filterDefinition).ToListAsync();
        }

        public async Task<Buyer> CreateBuyerAsync(Buyer buyer)
        {
            if (buyer.Id == Guid.Empty)
            {
                buyer.Id = Guid.NewGuid();
                buyer.CreatedAt = DateTime.Now;
            }

            await _buyers.InsertOneAsync(buyer);
            return buyer;
        }

        public async Task<bool> UpdateBuyerAsync(Guid id, Buyer buyerIn)
        {
            var existingBuyer = await _buyers.Find(b => b.Id == id).FirstOrDefaultAsync();
            if (existingBuyer == null)
            {
                return false;
            }

            buyerIn.Id = id;
            await _buyers.ReplaceOneAsync(b => b.Id == id, buyerIn);
            return true;
        }

        public async Task<object> ValidateFieldsAsync(Buyer buyer)
        {
            var emailExists = await _buyers.Find(b => b.Email == buyer.Email).FirstOrDefaultAsync();
            var cpfCnpjExists = await _buyers.Find(b => b.CpfCnpj == buyer.CpfCnpj).FirstOrDefaultAsync();

            return new
            {
                emailExists = emailExists != null,
                cpfCnpjExists = cpfCnpjExists != null,
            };
        }
    }
}
