using Xunit;
using Moq;
using AutoMapper;
using Microsoft.Extensions.Logging;
using Test.Server.DTOs;
using Test.Server.Models;
using Test.Server.Repositories;
using Test.Server.Services;

public class ProductServiceTests
{
    private readonly Mock<IProductRepository> _repoMock = new();
    private readonly Mock<IMapper> _mapperMock = new();
    private readonly Mock<ILogger<ProductService>> _loggerMock = new();

    private ProductService CreateService() =>
        new(_repoMock.Object, _mapperMock.Object, _loggerMock.Object);

    [Fact]
    public async Task AddProductAsync_ValidInput_ShouldSucceed()
    {
        var service = CreateService();

        _repoMock.Setup(r => r.AddProductAsync("Test")).ReturnsAsync(new Product { Id = 1, Name = "Test" });
        _mapperMock.Setup(m => m.Map<Product, ProductResponseDto>(It.IsAny<Product>()))
                   .Returns(new ProductResponseDto { Id = 1, Name = "Test" });

        var result = await service.AddProductAsync("Test");

        Assert.NotNull(result);
        Assert.Equal("Test", result.Name);
    }

    [Fact]
    public async Task AddPriceAsync_ValidInput_ShouldSucceed()
    {
        var service = CreateService();

        _repoMock.Setup(r => r.AddPriceAsync(1, 100))
                 .ReturnsAsync(new PriceDetail { Id = 1, Price = 100 });
        _mapperMock.Setup(m => m.Map<PriceDetail, PriceDetailResponseDto>(It.IsAny<PriceDetail>()))
                   .Returns(new PriceDetailResponseDto { Id = 1, Price = 100 });

        var result = await service.AddPriceAsync(1, 100);

        Assert.NotNull(result);
        Assert.Equal(100, result.Price);
    }

    [Fact]
    public async Task UpdatePriceAsync_ValidInput_ShouldSucceed()
    {
        var service = CreateService();

        _repoMock.Setup(r => r.UpdatePriceAsync(1, 120))
                 .ReturnsAsync(new PriceDetail { Id = 1, Price = 120 });
        _mapperMock.Setup(m => m.Map<PriceDetail, PriceDetailResponseDto>(It.IsAny<PriceDetail>()))
                   .Returns(new PriceDetailResponseDto { Id = 1, Price = 120 });

        var result = await service.UpdatePriceAsync(1, 120);

        Assert.NotNull(result);
        Assert.Equal(120, result.Price);
    }

    [Fact]
    public async Task DeleteProductAsync_ValidInput_ShouldSucceed()
    {
        var service = CreateService();

        _repoMock.Setup(r => r.DeleteProductAsync(1)).Returns(Task.CompletedTask);

        var exception = await Record.ExceptionAsync(() => service.DeleteProductAsync(1));
        Assert.Null(exception);
    }

    [Fact]
    public async Task SearchByNameAsync_ValidInput_ShouldSucceed()
    {
        var service = CreateService();

        _repoMock.Setup(r => r.SearchByNameAsync("milk"))
                 .ReturnsAsync(new List<Product> { new() { Id = 1, Name = "milk" } });
        _mapperMock.Setup(m => m.Map<IEnumerable<Product>, IEnumerable<ProductResponseDto>>(It.IsAny<IEnumerable<Product>>()))
                   .Returns(new List<ProductResponseDto> { new() { Id = 1, Name = "milk" } });

        var result = await service.SearchByNameAsync("milk");

        Assert.Single(result);
    }

    [Fact]
    public async Task SearchByPriceRangeAsync_ValidInput_ShouldSucceed()
    {
        var service = CreateService();

        _repoMock.Setup(r => r.SearchByPriceRangeAsync(10, 50))
                 .ReturnsAsync(new List<Product> { new() { Id = 1, Name = "test" } });
        _mapperMock.Setup(m => m.Map<IEnumerable<Product>, IEnumerable<ProductResponseDto>>(It.IsAny<IEnumerable<Product>>()))
                   .Returns(new List<ProductResponseDto> { new() { Id = 1, Name = "test" } });

        var result = await service.SearchByPriceRangeAsync(10, 50);

        Assert.Single(result);
    }

    [Fact]
    public async Task GetProductAsync_ValidId_ShouldReturnProduct()
    {
        var service = CreateService();

        _repoMock.Setup(r => r.GetProductAsync(1))
                 .ReturnsAsync(new Product { Id = 1, Name = "abc" });
        _mapperMock.Setup(m => m.Map<Product, ProductResponseDto>(It.IsAny<Product>()))
                   .Returns(new ProductResponseDto { Id = 1, Name = "abc" });

        var result = await service.GetProductAsync(1);

        Assert.NotNull(result);
        Assert.Equal("abc", result.Name);
    }
}