using Microsoft.AspNetCore.Mvc;
using MongoDB.Driver;
using SmartCommerceAPI.Data;
using SmartCommerceAPI.Models;

namespace SmartCommerceAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class BuyersController : ControllerBase
    {
        private readonly IMongoCollection<Buyer> _buyers;

        public BuyersController(MongoDbContext context)
        {
            _buyers = context.Buyers;
        }

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            try
            {
                const int pageSize = 20;

                var buyers = await _buyers.Find(FilterDefinition<Buyer>.Empty)
                                          .Limit(pageSize)
                                          .ToListAsync();

                return Ok(buyers);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [HttpGet("filter")]
        public async Task<IActionResult> FilterBuyers([FromQuery] BuyerFilter filter)
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

            var buyers = await _buyers.Find(filterDefinition).ToListAsync();

            return Ok(buyers);
        }

        [HttpPost]
        public async Task<ActionResult<Buyer>> Post([FromBody] Buyer buyer)
        {
            if (buyer.Id == Guid.Empty)
            {
                buyer.Id = Guid.NewGuid();
                buyer.CreatedAt = DateTime.Now;
            }

            await _buyers.InsertOneAsync(buyer);
            return StatusCode(StatusCodes.Status201Created, buyer);
        }

        [HttpPut("{id:guid}")]
        public async Task<IActionResult> Put(Guid id, [FromBody] Buyer buyerIn)
        {
            var buyer = await _buyers.Find(b => b.Id == id).FirstOrDefaultAsync();
            if (buyer == null)
            {
                return NotFound();
            }

            buyerIn.Id = id; 
            await _buyers.ReplaceOneAsync(b => b.Id == id, buyerIn);
            return NoContent();
        }
    }
}
