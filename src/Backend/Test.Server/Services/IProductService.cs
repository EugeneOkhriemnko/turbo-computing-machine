using Test.Server.DTOs;
using Test.Server.Models;

namespace Test.Server.Services;

public interface IProductService
{
    Task<ProductResponseDto> AddProductAsync(string name);
    Task<PriceDetailResponseDto> AddPriceAsync(int productId, decimal price);
    Task<PriceDetailResponseDto> UpdatePriceAsync(int priceId, decimal newPrice);
    Task DeleteProductAsync(int productId);
    Task<IEnumerable<ProductResponseDto>> SearchByNameAsync(string name);
    Task<IEnumerable<ProductResponseDto>> SearchByPriceRangeAsync(decimal min, decimal max);
    Task<ProductResponseDto?> GetProductAsync(int productId);
}
