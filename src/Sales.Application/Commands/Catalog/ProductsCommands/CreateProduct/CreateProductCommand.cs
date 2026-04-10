namespace Sales.Application.Commands.Catalog.ProductsCommands.CreateProduct;

public sealed class CreateProductCommand
{
    public string Name { get; }
    public string Code { get; }
    public decimal Price { get; }
    public Guid CategoryId { get; }
    public int InitialStock { get; }
    public string? Description { get; }
    public CreateProductCommand(string name, string code, decimal price, Guid categoryId, int initialStock = 0, string? description = null)
    {
        Name = name;
        Code = code;
        Price = price;
        CategoryId = categoryId;
        InitialStock = initialStock;
        Description = description;
    }
}
