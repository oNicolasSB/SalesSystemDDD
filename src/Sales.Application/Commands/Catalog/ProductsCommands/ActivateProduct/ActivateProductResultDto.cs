namespace Sales.Application.Commands.Catalog.ProductsCommands.ActivateProduct;

public class ActivateProductResultDto
{
    public Guid ProductId { get; init; }
    public string Status { get; init; } = string.Empty;
}
