using MongoDB.Driver;
using SmartCommerceAPI.Models;

namespace SmartCommerceAPI.Data
{
    public class MongoDbContext
    {
        private readonly IMongoDatabase _database = null;

        public MongoDbContext(IConfiguration configuration)
        {
            var client = new MongoClient(configuration.GetConnectionString("MongoDb"));
            _database = client.GetDatabase(configuration["MongoDbSettings:DatabaseName"]);
        }

        public IMongoCollection<Buyer> Buyers => _database.GetCollection<Buyer>("Buyers");
    }
}
