namespace Sales.Application.Commands.Catalog.ProductsCommands.UpdateProductName;

public sealed class UpdateProductNameResultDto
{
    public Guid ProductId { get; init; }
    public string Name { get; init; } = default!;
}
