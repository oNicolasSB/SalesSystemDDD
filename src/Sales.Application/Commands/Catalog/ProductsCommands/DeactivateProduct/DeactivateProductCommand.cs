namespace Sales.Application.Commands.Catalog.ProductsCommands.DeactivateProduct;

public sealed class DeactivateProductCommand
{
    public Guid ProductId { get; }

    public DeactivateProductCommand(Guid productId)
    {
        ProductId = productId;
    }
}
