using Microsoft.Extensions.Options;
using MongoDB.Bson;
using MongoDB.Driver;
using SmartCommerceAPI.Application.Interfaces;
using SmartCommerceAPI.Models;

namespace SmartCommerceAPI.Application.Services
{
    public class BuyerService : IBuyerService
    {
        private readonly IMongoCollection<Buyer> _buyersCollection;

        public BuyerService(IMongoClient mongoClient, IOptions<MongoDbSettings> mongoDbSettings)
        {
            var database = mongoClient.GetDatabase(mongoDbSettings.Value.DatabaseName);
            _buyersCollection = database.GetCollection<Buyer>("Buyers");
        }

        public async Task<Buyer> GetByIdAsync(string id)
        {
            return await _buyersCollection.Find(b => b.Id == new Guid(id)).FirstOrDefaultAsync();
        }

        public async Task<IEnumerable<Buyer>> GetAllAsync()
        {
            return await _buyersCollection.Find(_ => true).ToListAsync();
        }

        public async Task CreateAsync(Buyer buyer)
        {
            await _buyersCollection.InsertOneAsync(buyer);
        }

        public async Task UpdateAsync(string id, Buyer buyer)
        {
            await _buyersCollection.ReplaceOneAsync(b => b.Id == new Guid(id), buyer);
        }

        public async Task DeleteAsync(string id)
        {
            await _buyersCollection.DeleteOneAsync(b => b.Id == new Guid(id));
        }
    }
}
