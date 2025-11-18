using AutoMapper;
using Microsoft.Extensions.Logging;
using Test.Server.DTOs;
using Test.Server.Models;
using Test.Server.Repositories;

namespace Test.Server.Services
{
    public class PriceService : IPriceService
    {
        private readonly IProductRepository _repository;
        private readonly IMapper _mapper;
        private readonly ILogger<PriceService> _logger;

        public PriceService(
            IProductRepository repository,
            IMapper mapper,
            ILogger<PriceService> logger)
        {
            _repository = repository;
            _mapper = mapper;
            _logger = logger;
        }

        public virtual async Task<PriceDetailResponseDto> AddPriceAsync(int productId, decimal price)
        {
            try
            {
                _logger.LogInformation("Adding price {price} for product ID {productId}", price, productId);
                if (price <= 0)
                {
                    _logger.LogWarning("Price must be greater than zero.");
                    throw new ArgumentException("Price must be greater than zero.", nameof(price));
                }
                return _mapper.Map<PriceDetail, PriceDetailResponseDto>(await _repository.AddPriceAsync(productId, price));
            }
            catch
            {
                _logger.LogError("Error occurred while adding price {price} for product ID {productId}", price, productId);
                throw;
            }
        }

        public async Task<PriceDetailResponseDto> UpdatePriceAsync(int priceId, decimal newPrice)
        {
            try
            {
                _logger.LogInformation("Updating price ID {priceId} to new price {newPrice}", priceId, newPrice);
                if (newPrice <= 0)
                {
                    _logger.LogWarning("New price must be greater than zero.");
                    throw new ArgumentException("New price must be greater than zero.", nameof(newPrice));
                }
                return _mapper.Map<PriceDetail, PriceDetailResponseDto>(await _repository.UpdatePriceAsync(priceId, newPrice));
            }
            catch
            {
                _logger.LogError("Error occurred while updating price ID {priceId} to new price {newPrice}", priceId, newPrice);
                throw;
            }
        }

        public virtual async Task DeletePriceAsync(int priceId)
        {
            try
            {
                _logger.LogInformation("Deleting price ID {priceId}", priceId);
                await _repository.DeletePriceAsync(priceId);
            }
            catch
            {
                _logger.LogError("Error occurred while deleting price ID {priceId}", priceId);
                throw;
            }
        }

        public virtual async Task<IList<PriceDetailResponseDto>> EvaluatePricesAsync(int productId, string segment)
        {
            _logger.LogInformation("Retrieving prices for product ID {productId}", productId);
            var prices = await _repository.GetProductPricesAsync(productId);

            prices = [.. prices.Where(p => p.Segment == segment)];

            return _mapper.Map<IList<PriceDetail>, IList<PriceDetailResponseDto>>(prices ?? []);
        }

        public virtual async Task<IList<PriceDetailResponseDto>> GetPricesByProductAsync(int productId)
        {
            try
            {
                _logger.LogInformation("Retrieving prices for product ID {productId}", productId);
                var prices = await _repository.GetProductPricesAsync(productId);
                return _mapper.Map<IList<PriceDetail>, IList<PriceDetailResponseDto>>(prices);
            }
            catch
            {
                _logger.LogError("Error occurred while retrieving prices for product ID {productId}", productId);
                throw;
            }
        }
    }
}
