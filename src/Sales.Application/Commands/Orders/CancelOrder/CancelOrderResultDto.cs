namespace Sales.Application.Commands.Orders.CancelOrder;

public sealed class CancelOrderResultDto
{
    public Guid OrderId { get; init; }
    public string OrderStatus { get; init; } = string.Empty;
}
