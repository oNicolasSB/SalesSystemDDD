namespace Sales.Application.Commands.Orders.RemoveOrderItem;

public sealed class RemoveOrderItemCommand
{
    public Guid OrderId { get; }
    public Guid ItemId { get; }

    public RemoveOrderItemCommand(Guid orderId, Guid itemId)
    {
        OrderId = orderId;
        ItemId = itemId;
    }
}
