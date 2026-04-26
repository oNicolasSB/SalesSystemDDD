namespace Sales.Application.Commands.OrdersCommands.AddOrderItem;

public sealed class AddOrderItemCommand
{
    public Guid OrderId { get; }
    public Guid ProductId { get; }
    public int Quantity { get; }

    public AddOrderItemCommand(
        Guid orderId,
        Guid productId,
        int quantity
    )
    {
        OrderId = orderId;
        ProductId = productId;
        Quantity = quantity;
    }
}
