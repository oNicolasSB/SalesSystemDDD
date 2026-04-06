namespace Sales.Application.Commands.Orders.MarkAsDelivered;

public class MarkAsShippedResultDto
{
    public Guid OrderId { get; init; }
    public string OrderStatus { get; init; } = string.Empty;
}
