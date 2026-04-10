namespace Sales.Application.Commands.Catalog.ProductsCommands.UpdateStock;

public sealed class UpdateStockResultDto
{
    public Guid ProductId { get; init; }
    public int CurrentStock { get; init; }
}
