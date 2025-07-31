namespace Test.Server.Models;

public class PriceDetail
{
    public int Id { get; set; }
    public int ProductId { get; set; }
    public decimal Price { get; set; }
    public DateTime CreatedDate { get; set; }
    public virtual Product Product { get; set; } = null!;
}
