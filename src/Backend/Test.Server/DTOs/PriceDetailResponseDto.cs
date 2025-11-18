namespace Test.Server.DTOs;

public record PriceDetailResponseDto
{
    public int Id { get; set; }
    public int ProductId { get; set; }
    public decimal Price { get; set; }
    public string Segment { get; set; }
    public DateTime CreatedDate { get; set; }
    public DateTime From { get; set; }
    public DateTime To { get; set; }
}
