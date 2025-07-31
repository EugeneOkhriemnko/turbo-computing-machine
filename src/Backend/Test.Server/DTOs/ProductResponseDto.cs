namespace Test.Server.DTOs;

public record ProductResponseDto
{
    public int Id { get; init; }
    public string Name { get; init; } = string.Empty;
    public List<PriceDetailResponseDto> PriceDetails { get; init; } = [];
}
