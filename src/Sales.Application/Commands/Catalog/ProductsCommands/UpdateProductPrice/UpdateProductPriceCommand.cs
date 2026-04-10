namespace Sales.Application.Commands.Catalog.ProductsCommands.UpdateProductPrice;

public sealed class UpdateProductPriceCommand
{
    public Guid ProductId { get; }
    public decimal NewPrice { get; }

    public UpdateProductPriceCommand(Guid productId, decimal newPrice)
    {
        ProductId = productId;
        NewPrice = newPrice;
    }
}
