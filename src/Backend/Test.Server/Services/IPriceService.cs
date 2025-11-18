using Test.Server.DTOs;
using Test.Server.Models;

namespace Test.Server.Services
{
    public interface IPriceService
    {
        Task<IList<PriceDetailResponseDto>> GetPricesByProductAsync(int productId);
        Task<PriceDetailResponseDto> AddPriceAsync(int productId, decimal price);
        Task<PriceDetailResponseDto> UpdatePriceAsync(int priceId, decimal newPrice);
        Task DeletePriceAsync(int priceId);
        Task<IList<PriceDetail>> EvaluatePricesAsync(int productId, string segment);
    }
}
