namespace Sales.Domain.Orders.Integration.Catalog;

public sealed class ProductDto
{
    public Guid Id { get; }
    public string Name { get; }
    public decimal Price { get; }

    public ProductDto(Guid id, string name, decimal price)
    {
        Id = id;
        Name = name;
        Price = price;
    }
}
