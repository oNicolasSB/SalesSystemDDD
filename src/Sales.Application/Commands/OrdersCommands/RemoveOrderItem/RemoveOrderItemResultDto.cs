namespace Sales.Application.Commands.OrdersCommands.RemoveOrderItem;

public sealed class RemoveOrderItemResultDto
{
    public Guid OrderId { get; }
    public decimal TotalValue { get; }
    public string OrderStatus { get; }

    public RemoveOrderItemResultDto(Guid orderId, decimal totalValue, string orderStatus)
    {
        OrderId = orderId;
        TotalValue = totalValue;
        OrderStatus = orderStatus;
    }
}
