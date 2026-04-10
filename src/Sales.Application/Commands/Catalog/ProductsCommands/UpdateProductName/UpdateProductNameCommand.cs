namespace Sales.Application.Commands.Catalog.ProductsCommands.UpdateProductName;

public sealed class UpdateProductNameCommand
{
    public Guid ProductId { get; }
    public string NewName { get; }

    public UpdateProductNameCommand(Guid productId, string newName)
    {
        ProductId = productId;
        NewName = newName;
    }
}
