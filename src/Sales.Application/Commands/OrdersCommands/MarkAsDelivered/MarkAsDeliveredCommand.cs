namespace Sales.Application.Commands.OrdersCommands.MarkAsDelivered;

public sealed class MarkAsDeliveredCommand
{
    public Guid OrderId { get; init; }

    public MarkAsDeliveredCommand(Guid orderId)
    {
        OrderId = orderId;
    }
}
