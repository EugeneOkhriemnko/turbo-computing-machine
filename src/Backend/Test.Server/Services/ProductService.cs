using AutoMapper;
using Microsoft.Extensions.Logging;
using Test.Server.DTOs;
using Test.Server.Models;
using Test.Server.Repositories;

namespace Test.Server.Services;

public class ProductService(
    IProductRepository repository,
    IMapper mapper,
    ILogger<ProductService> logger) : IProductService
{
    private readonly IProductRepository _repository = repository;
    private readonly IMapper _mapper = mapper;
    private readonly ILogger<ProductService> _logger = logger;

    public async Task<ProductResponseDto> AddProductAsync(string name)
    {
        try
        {
            _logger.LogInformation("Adding product with name: {productName}", name);
            if (string.IsNullOrWhiteSpace(name))
            {
                _logger.LogWarning("Product name cannot be null or empty.");
                throw new ArgumentException("Product name cannot be null or empty.", nameof(name));
            }
            return _mapper.Map<Product, ProductResponseDto>(await _repository.AddProductAsync(name));
        }
        catch
        {
            _logger.LogError("Error occurred while adding product with name: {productName}", name);
            throw;
        }
    }

    public async Task DeleteProductAsync(int productId)
    {
        try
        {
            _logger.LogInformation("Deleting product with ID {productId}", productId);
            if (productId <= 0)
            {
                _logger.LogWarning("Product ID must be greater than zero.");
                throw new ArgumentException("Product ID must be greater than zero.", nameof(productId));
            }
            await _repository.DeleteProductAsync(productId);
        }
        catch
        {
            _logger.LogError("Error occurred while deleting product with ID {productId}", productId);
            throw;
        }
    }

    public async Task<IEnumerable<ProductResponseDto>> SearchByNameAsync(string name)
    {
        try
        {
            _logger.LogInformation("Searching products by name: {productName}", name);
            if (string.IsNullOrWhiteSpace(name))
            {
                _logger.LogWarning("Search name cannot be null or empty.");
                throw new ArgumentException("Search name cannot be null or empty.", nameof(name));
            }
            return _mapper.Map<IEnumerable<Product>, IEnumerable<ProductResponseDto>>(
                await _repository.SearchProductsByNameAsync(name));
        }
        catch
        {
            _logger.LogError("Error occurred while searching products by name: {productName}", name);
            throw;
        }
    }

    public async Task<IEnumerable<ProductResponseDto>> SearchByPriceRangeAsync(decimal min, decimal max)
    {
        try
        {
            _logger.LogInformation("Searching products by price range: {min} - {max}", min, max);
            if (min < 0 || max < 0 || min > max)
            {
                _logger.LogWarning("Invalid price range: {min} - {max}", min, max);
                throw new ArgumentException("Invalid price range.", nameof(min));
            }
            return _mapper.Map<IEnumerable<Product>, IEnumerable<ProductResponseDto>>(
                await _repository.SearchProductsByPriceRangeAsync(min, max));
        }
        catch
        {
            _logger.LogError("Error occurred while searching products by price range: {min} - {max}", min, max);
            throw;
        }
    }

    public async Task<ProductResponseDto?> GetProductAsync(int productId)
    {
        try
        {
            _logger.LogInformation("Retrieving product with ID {productId} and its prices", productId);
            if (productId <= 0)
            {
                _logger.LogWarning("Product ID must be greater than zero.");
                throw new ArgumentException("Product ID must be greater than zero.", nameof(productId));
            }
            var product = await _repository.GetProductAsync(productId);
            return product is not null
                ? _mapper.Map<Product, ProductResponseDto>(product)
                : default;
        }
        catch
        {
            _logger.LogError("Error occurred while retrieving product with ID {productId}", productId);
            throw;
        }
    }
}
