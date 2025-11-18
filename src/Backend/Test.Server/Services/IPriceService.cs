using Test.Server.DTOs;

namespace Test.Server.Services
{
    public interface IPriceService
    {
        Task<IList<PriceDetailResponseDto>> GetPricesByProductAsync(int productId);
        Task<PriceDetailResponseDto> AddPriceAsync(int productId, decimal price);
        Task<PriceDetailResponseDto> UpdatePriceAsync(int priceId, decimal newPrice);
        Task DeletePriceAsync(int priceId);
        Task<IList<PriceDetailResponseDto>> EvaluatePricesAsync(int productId, string segment);
    }
}
