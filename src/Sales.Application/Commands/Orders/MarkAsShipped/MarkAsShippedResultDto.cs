namespace Sales.Application.Commands.Orders.MarkAsShipped;

public sealed class MarkAsShippedResultDto
{
    public Guid OrderId { get; init; }
    public string OrderStatus { get; init; } = string.Empty;
}
