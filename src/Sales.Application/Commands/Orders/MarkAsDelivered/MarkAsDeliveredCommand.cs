namespace Sales.Application.Commands.Orders.MarkAsDelivered;

public sealed class MarkAsDeliveredCommand
{
    public Guid OrderId { get; init; }

    public MarkAsDeliveredCommand(Guid orderId)
    {
        OrderId = orderId;
    }
}
