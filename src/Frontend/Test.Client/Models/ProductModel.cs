namespace Test.Client.Models;

record class ProductModel
{
    public int Id { get; init; }
    public string Name { get; init; } = string.Empty;
    public List<PriceDetailModel> PriceDetails { get; init; } = [];
}
