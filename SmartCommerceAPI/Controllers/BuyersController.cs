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

        [HttpGet("{id:guid}", Name = "GetBuyer")]
        public async Task<ActionResult<Buyer>> Get(Guid id)
        {
            var buyer = await _buyers.Find(b => b.Id == id).FirstOrDefaultAsync();
            if (buyer == null)
            {
                return NotFound();
            }
            return Ok(buyer);
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
            return CreatedAtRoute("GetBuyer", new { id = buyer.Id }, buyer);
        }

        [HttpPut("{id:guid}")]
        public async Task<IActionResult> Put(Guid id, [FromBody] Buyer buyerIn)
        {
            var buyer = await _buyers.Find(b => b.Id == id).FirstOrDefaultAsync();
            if (buyer == null)
            {
                return NotFound();
            }

            // Replace the existing document with the new one
            buyerIn.Id = id; // Ensure ID remains unchanged
            await _buyers.ReplaceOneAsync(b => b.Id == id, buyerIn);
            return NoContent();
        }

        [HttpDelete("{id:guid}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            var result = await _buyers.DeleteOneAsync(b => b.Id == id);
            if (result.DeletedCount == 0)
            {
                return NotFound();
            }
            return NoContent();
        }
    }
}
