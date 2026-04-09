namespace Sales.Application.Commands.OrdersCommands.MarkAsShipped;

public sealed class MarkAsShippedCommand
{
    public Guid OrderId { get; init; }

    public MarkAsShippedCommand(Guid orderId)
    {
        OrderId = orderId;
    }
}
