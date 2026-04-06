namespace Sales.Application.Commands.Orders.MarkAsShipped;

public sealed class MarkAsShippedCommand
{
    public Guid OrderId { get; init; }

    public MarkAsShippedCommand(Guid orderId)
    {
        OrderId = orderId;
    }
}
