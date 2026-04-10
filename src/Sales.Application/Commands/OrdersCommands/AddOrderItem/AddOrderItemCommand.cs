namespace Sales.Application.Commands.OrdersCommands.AddOrderItem;

public sealed class AddOrderItemCommand
{
    public Guid OrderId { get; }
    public Guid ProductId { get; }
    public string ProductName { get; }
    public decimal UnitPrice { get; }
    public int Quantity { get; }

    public AddOrderItemCommand(
        Guid orderId,
        Guid productId,
        string productName,
        decimal unitPrice,
        int quantity
    )
    {
        OrderId = orderId;
        ProductId = productId;
        ProductName = productName;
        UnitPrice = unitPrice;
        Quantity = quantity;
    }
}
