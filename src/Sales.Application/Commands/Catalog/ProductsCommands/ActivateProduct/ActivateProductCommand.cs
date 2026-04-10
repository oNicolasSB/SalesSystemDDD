namespace Sales.Application.Commands.Catalog.ProductsCommands.ActivateProduct;

public sealed class ActivateProductCommand
{
    public Guid ProductId { get; }

    public ActivateProductCommand(Guid productId)
    {
        ProductId = productId;
    }
}
