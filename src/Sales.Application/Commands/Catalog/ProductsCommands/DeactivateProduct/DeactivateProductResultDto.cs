namespace Sales.Application.Commands.Catalog.ProductsCommands.DeactivateProduct;

public class DeactivateProductResultDto
{
    public Guid ProductId { get; init; }
    public string Status { get; init; } = string.Empty;
}
