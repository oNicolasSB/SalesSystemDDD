namespace Sales.Application.Commands.Catalog.ProductsCommands.UpdateStock;

public sealed class UpdateStockCommand
{

    public Guid ProductId { get; }
    public int Quantity { get; }
    public string Reason { get; }
    public UpdateStockCommand(Guid productId, int quantity, string reason)
    {
        ProductId = productId;
        Quantity = quantity;
        Reason = reason;
    }
}
