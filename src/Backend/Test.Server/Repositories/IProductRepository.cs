using Test.Server.Models;

namespace Test.Server.Repositories;

public interface IProductRepository
{
    Task<Product> AddProductAsync(string name);
    Task<PriceDetail> AddPriceAsync(int productId, decimal price);
    Task<PriceDetail> UpdatePriceAsync(int priceId, decimal newPrice);
    Task DeleteProductAsync(int productId);
    Task<IEnumerable<Product>> SearchByNameAsync(string name);
    Task<IEnumerable<Product>> SearchByPriceRangeAsync(decimal min, decimal max);
    Task<Product?> GetProductAsync(int productId);
}
