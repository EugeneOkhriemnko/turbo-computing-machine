using Test.Server.Models;

namespace Test.Server.Repositories;

public interface IProductRepository
{


    Task<Product> AddProductAsync(string name);
    Task DeleteProductAsync(int productId);
    Task<IEnumerable<Product>> SearchProductsByNameAsync(string name);
    Task<IEnumerable<Product>> SearchProductsByPriceRangeAsync(decimal min, decimal max);
    Task<Product?> GetProductAsync(int productId);

    Task<PriceDetail> AddPriceAsync(int productId, decimal price);
    Task<PriceDetail> UpdatePriceAsync(int priceId, decimal newPrice);
    Task<IList<PriceDetail>> GetProductPricesAsync(int productId);
    Task DeletePriceAsync(int priceId);
}
