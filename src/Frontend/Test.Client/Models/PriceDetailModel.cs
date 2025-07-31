namespace Test.Client.Models;

internal record PriceDetailModel
{
    public int Id { get; set; }
    public int ProductId { get; set; }
    public decimal Price { get; set; }
    public DateTime CreatedDate { get; set; }
}
