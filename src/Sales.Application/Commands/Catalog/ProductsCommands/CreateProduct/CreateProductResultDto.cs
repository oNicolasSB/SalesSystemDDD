namespace Sales.Application.Commands.Catalog.ProductsCommands.CreateProduct;

public class CreateProductResultDto
{
    public Guid ProductId { get; set; }
    public string Name { get; set; } = string.Empty;
    public decimal Price { get; set; }
    public string Status { get; set; } = string.Empty;
}
