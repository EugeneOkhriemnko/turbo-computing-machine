using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Test.Server.Data;
using Test.Server.Models;

namespace Test.Server.Repositories;

public class ProductRepository(DataContext context, ILogger<ProductRepository> logger) : IProductRepository
{
    private readonly DataContext _context = context;
    private ILogger<ProductRepository> _logger = logger;

    public async Task<PriceDetail> AddPriceAsync(int productId, decimal price)
    {
        try
        {
            _logger.LogInformation("Adding price {price} for product ID {productId}", price, productId);
            var product = await _context.Products.FindAsync(productId);
            if (product is null)
            {
                _logger.LogWarning("Product with ID {productId} not found.", productId);
                throw new KeyNotFoundException($"Product with ID {productId} not found.");
            }
            var priceDetail = new PriceDetail
            {
                ProductId = productId,
                Price = price,
                CreatedDate = DateTime.UtcNow
            };
            _context.PriceDetails.Add(priceDetail);
            await _context.SaveChangesAsync();
            _logger.LogInformation("Price {price} added successfully for product ID {productId}", price, productId);
            return priceDetail;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error adding price {price} for product ID {productId}", price, productId);
            throw;
        }
    }

    public async Task<PriceDetail> UpdatePriceAsync(int priceId, decimal newPrice)
    {
        try
        {
            _logger.LogInformation("Updating price with ID {priceId} to new price {newPrice}", priceId, newPrice);
            var price = await _context.PriceDetails.FindAsync(priceId);
            if (price is null)
            {
                _logger.LogWarning("Price with ID {priceId} not found.", priceId);
                throw new KeyNotFoundException($"Price with ID {priceId} not found.");
            }
            price.Price = newPrice;
            await _context.SaveChangesAsync();
            _logger.LogInformation("Price with ID {priceId} updated successfully to {newPrice}", priceId, newPrice);
            return price;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error updating price with ID {priceId} to new price {newPrice}", priceId, newPrice);
            throw;
        }
    }

    public async Task<IList<PriceDetail>> GetProductPricesAsync(int productId)
    {
        try
        {
            _logger.LogInformation("Loading prices for product with ID {productId}", productId);
            var prices = await _context.PriceDetails.Where(x => x.ProductId == productId).ToListAsync();
            if (!prices.Any())
            {
                _logger.LogWarning("No prices found for product with ID {productId}.", productId);
                throw new KeyNotFoundException($"No prices found for product with ID {productId}.");
            }
            return prices ?? [];
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving prices for product with ID {productId}", productId);
            throw;
        }
    }

    public async Task DeletePriceAsync(int priceId)
    {
        try
        {
            var price = await _context.PriceDetails.FindAsync(priceId);
            if (price is null)
            {
                _logger.LogWarning("Price with ID {priceId} not found.", priceId);
                throw new KeyNotFoundException($"Price with ID {priceId} not found.");
            }
            _context.PriceDetails.Remove(price);
            await _context.SaveChangesAsync();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error deleting price with ID {priceId}", priceId);
            throw;
        }
    }

    public async Task<Product> AddProductAsync(string name)
    {
        try
        {
            _logger.LogInformation("Adding product: {ProductName}", name);
            var product = new Product { Name = name };
            _context.Products.Add(product);
            await _context.SaveChangesAsync();
            _logger.LogInformation("Product added successfully: {ProductId}", product.Id);
            return product;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error adding product: {ProductName}", name);
            throw;
        }
    }

    public async Task DeleteProductAsync(int productId)
    {
        try
        {
            var product = await _context.Products.FindAsync(productId);
            if (product is null)
            {
                _logger.LogWarning("Product with ID {productId} not found.", productId);
                throw new KeyNotFoundException($"Product with ID {productId} not found.");
            }

            _context.Products.Remove(product);
            await _context.SaveChangesAsync();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error deleting product with ID {productId}", productId);
            throw;
        }
    }

    public async Task<IEnumerable<Product>> SearchProductsByNameAsync(string name)
    {
        try
        {
            _logger.LogInformation("Searching products by name: {productName}", name);
            var products = await _context.Products
                .Where(p => EF.Functions.Like(p.Name, $"%{name}%"))
                .ToListAsync();
            _logger.LogInformation("Found {count} products matching name: {productName}", products.Count, name);
            return products;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error searching products by name: {productName}", name);
            throw;
        }
    }

    public async Task<IEnumerable<Product>> SearchProductsByPriceRangeAsync(decimal min, decimal max)
    {
        try
        {
            _logger.LogInformation("Searching products by price range: {min} - {max}", min, max);
            var products = await _context.Products
                .Where(p => p.PriceDetails.Any(pd => pd.Price >= min && pd.Price <= max))
                .ToListAsync();
            _logger.LogInformation("Found {count} products in price range: {min} - {max}", products.Count, min, max);
            return products;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error searching products by price range: {min} - {max}", min, max);
            throw;
        }
    }

    public async Task<Product?> GetProductAsync(int productId)
    {
        _logger.LogInformation("Retrieving product with ID {productId} and its prices", productId);
        return await _context.Products.FirstOrDefaultAsync(p => p.Id == productId);
    }
}
