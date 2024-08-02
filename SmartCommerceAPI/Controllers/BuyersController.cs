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
        private readonly ILogger<BuyersController> _logger;

        public BuyersController(MongoDbContext context, ILogger<BuyersController> logger)
        {
            _buyers = context.Buyers;
            _logger = logger;
        }

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            try
            {
                const int pageSize = 20;

                _logger.LogInformation("Fetching buyers with a limit of {PageSize}", pageSize);

                var buyers = await _buyers.Find(FilterDefinition<Buyer>.Empty)
                                          .Limit(pageSize)
                                          .ToListAsync();

                _logger.LogInformation("Fetched {Count} buyers", buyers.Count);

                return Ok(buyers);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while fetching buyers");
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [HttpGet("filter")]
        public async Task<IActionResult> FilterBuyers([FromQuery] BuyerFilter filter)
        {
            try
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

                _logger.LogInformation("Filtering buyers with the provided filter");

                var buyers = await _buyers.Find(filterDefinition).ToListAsync();

                _logger.LogInformation("Fetched {Count} buyers with filter", buyers.Count);

                return Ok(buyers);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while filtering buyers");
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [HttpPost]
        public async Task<ActionResult<Buyer>> Post([FromBody] Buyer buyer)
        {
            try
            {
                if (buyer.Id == Guid.Empty)
                {
                    buyer.Id = Guid.NewGuid();
                    buyer.CreatedAt = DateTime.Now;
                }

                _logger.LogInformation("Creating a new buyer with ID {BuyerId}", buyer.Id);

                await _buyers.InsertOneAsync(buyer);

                _logger.LogInformation("Buyer created successfully with ID {BuyerId}", buyer.Id);

                return StatusCode(StatusCodes.Status201Created, buyer);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while creating a buyer");
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [HttpPut("{id:guid}")]
        public async Task<IActionResult> Put(Guid id, [FromBody] Buyer buyerIn)
        {
            try
            {
                var buyer = await _buyers.Find(b => b.Id == id).FirstOrDefaultAsync();
                if (buyer == null)
                {
                    _logger.LogWarning("Buyer with ID {BuyerId} not found", id);
                    return NotFound();
                }

                buyerIn.Id = id;
                await _buyers.ReplaceOneAsync(b => b.Id == id, buyerIn);

                _logger.LogInformation("Buyer with ID {BuyerId} updated successfully", id);

                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while updating a buyer with ID {BuyerId}", id);
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [HttpPost("validate-fields")]
        public async Task<IActionResult> ValidateFields([FromBody] Buyer buyer)
        {
            try
            {
                var emailExists = await _buyers.Find(b => b.Email == buyer.Email).FirstOrDefaultAsync();
                var cpfCnpjExists = await _buyers.Find(b => b.CpfCnpj == buyer.CpfCnpj).FirstOrDefaultAsync();
                

                _logger.LogInformation("Validating fields for buyer with email {Email} and CPF/CNPJ {CpfCnpj}", buyer.Email, buyer.CpfCnpj);

                return Ok(new
                {
                    emailExists = emailExists != null,
                    cpfCnpjExists = cpfCnpjExists != null,
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while validating fields for buyer with email {Email} and CPF/CNPJ {CpfCnpj}", buyer.Email, buyer.CpfCnpj);
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }
    }
}
