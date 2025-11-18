using Test.Server.DTOs;

namespace Test.Server.Services;

public interface IProductService
{
    Task<ProductResponseDto> AddProductAsync(string name);
    Task DeleteProductAsync(int productId);
    Task<IEnumerable<ProductResponseDto>> SearchByNameAsync(string name);
    Task<IEnumerable<ProductResponseDto>> SearchByPriceRangeAsync(decimal min, decimal max);
    Task<ProductResponseDto?> GetProductAsync(int productId);
}
