using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Test.Server.DTOs;
using Test.Server.Services;

namespace Test.Server.Controllers;

[ApiController]
[Route("api/products")]
[Authorize]
public class ProductController(IProductService service, ILogger<ProductController> logger) : Controller
{
    private readonly IProductService _service = service;
    private readonly ILogger<ProductController> _logger = logger;

    [HttpPost]
    public async Task<IActionResult> AddProduct([FromBody] ProductDto dto)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        try
        {
            _logger.LogInformation("Adding product with name: {productName}", dto.Name);
            var product = await _service.AddProductAsync(dto.Name);
            return CreatedAtAction(nameof(GetProduct), new { id = product.Id }, product);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error occurred while adding product with name: {productName}", dto.Name);
            return StatusCode(500, "Internal server error");
        }
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetProduct(int id)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        try
        {
            _logger.LogInformation("Fetching product with ID: {productId}", id);
            var product = await _service.GetProductAsync(id);
            return Ok(product);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error fetching product with ID: {productId}", id);
            return StatusCode(500, "Internal server error");
        }
    }

    [HttpPost("{productId}/prices")]
    public async Task<IActionResult> AddPrice(int productId, [FromBody] PriceDetailDto dto)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        try
        {
            _logger.LogInformation("Adding price for product ID: {productId} with price: {price}", productId, dto.Price);
            var price = await _service.AddPriceAsync(productId, dto.Price);
            return Ok(price);
        }
        catch (ArgumentException ex)
        {
            _logger.LogWarning(ex, "Invalid price for product ID: {productId}", productId);
            return BadRequest($"Invalid price for product ID: {productId}");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error adding price for product ID: {productId}", productId);
            return StatusCode(500, "Internal server error");
        }
    }

    [HttpPut("price/{priceId}")]
    public async Task<IActionResult> UpdatePrice(int priceId, [FromBody] PriceDetailDto dto)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        try
        {
            _logger.LogInformation("Updating price ID: {priceId} to new price: {newPrice}", priceId, dto.Price);
            var price = await _service.UpdatePriceAsync(priceId, dto.Price);
            return Ok(price);
        }
        catch (KeyNotFoundException ex)
        {
            _logger.LogWarning(ex, "Price with ID: {priceId} not found", priceId);
            return NotFound($"Price with ID: {priceId} not found");
        }
        catch (ArgumentException ex)
        {
            _logger.LogWarning(ex, "Invalid price update for ID: {priceId}", priceId);
            return BadRequest($"Invalid price update for ID: {priceId}");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error updating price ID: {priceId}", priceId);
            return StatusCode(500, "Internal server error");
        }
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteProduct(int id)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        try
        {
            _logger.LogInformation("Deleting product with ID: {productId}", id);
            await _service.DeleteProductAsync(id);
            return NoContent();
        }
        catch (KeyNotFoundException ex)
        {
            _logger.LogWarning(ex, "Product with ID: {productId} not found", id);
            return NotFound($"Product with ID: {id} not found");
        }
        catch (ArgumentException ex)
        {
            _logger.LogWarning(ex, "Invalid product ID: {productId}", id);
            return BadRequest($"Invalid product ID: {id}");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error deleting product with ID: {productId}", id);
            return StatusCode(500, "Internal server error");
        }        
    }

    [HttpGet("search/{name}")]
    public async Task<IActionResult> SearchByName(string name)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        try
        {
            _logger.LogInformation("Searching products by name: {productName}", name);
            var products = await _service.SearchByNameAsync(name);
            return Ok(products);
        }
        catch (ArgumentException ex)
        {
            _logger.LogWarning(ex, "Invalid product name: {productName}", name);
            return BadRequest($"Invalid product name: {name}");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error searching products by name: {productName}", name);
            return StatusCode(500, "Internal server error");

        }
    }

    [HttpGet("search")]
    public async Task<IActionResult> SearchByPrice([FromQuery] decimal min, [FromQuery] decimal max)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        try
        {
            _logger.LogInformation("Searching products by price range: {minPrice} - {maxPrice}", min, max);
            var products = await _service.SearchByPriceRangeAsync(min, max);
            return Ok(products);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error searching products by price range: {minPrice} - {maxPrice}", min, max);
            return StatusCode(500, "Internal server error");
        }
    }
}
