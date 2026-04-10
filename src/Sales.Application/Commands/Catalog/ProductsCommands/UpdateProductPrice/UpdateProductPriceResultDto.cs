namespace Sales.Application.Commands.Catalog.ProductsCommands.UpdateProductPrice;

public sealed class UpdateProductPriceResultDto
{
    public Guid ProductId { get; init; }
    public decimal NewPrice { get; init; }
}
