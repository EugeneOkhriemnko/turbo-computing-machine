using AutoMapper;
using Microsoft.Extensions.Logging;
using Test.Server.DTOs;
using Test.Server.Models;
using Test.Server.Repositories;
using Test.Server.Services;

namespace Test.Server.ExtendedServices
{
    public class ExtendedPriceService : PriceService
    {
        private readonly IProductRepository _repository;
        private readonly IMapper _mapper;
        private readonly ILogger<ExtendedPriceService> _logger;

        public ExtendedPriceService(IProductRepository repository, IMapper mapper, ILogger<ExtendedPriceService> logger) : base(repository, mapper, logger)
        {
            _repository = repository;
            _mapper = mapper;
            _logger = logger;
        }

        public override async Task<IList<PriceDetailResponseDto>> EvaluatePricesAsync(int productId, string segment)
        {
            _logger.LogInformation("Retrieving prices for product ID {productId}", productId);
            var prices = await _repository.GetProductPricesAsync(productId);

            prices = [.. prices.Where(p => p.Segment == segment).Where(p => p.From < DateTime.UtcNow && p.To > DateTime.UtcNow)];

            return _mapper.Map<IList<PriceDetail>, IList<PriceDetailResponseDto>>(prices ?? []);
        }
    }
}
