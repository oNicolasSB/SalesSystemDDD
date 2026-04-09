namespace Sales.Application.Commands.OrdersCommands.MarkAsShipped;

public sealed class MarkAsShippedResultDto
{
    public Guid OrderId { get; init; }
    public string OrderStatus { get; init; } = string.Empty;
}
