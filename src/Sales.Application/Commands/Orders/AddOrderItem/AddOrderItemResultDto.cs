namespace Sales.Application.Commands.Orders.AddOrderItem;

public sealed class AddOrderItemResultDto
{
    public Guid OrderId { get; }
    public decimal TotalValue { get; }
    public string OrderStatus { get; }

    public AddOrderItemResultDto(Guid orderId, decimal totalValue, string orderStatus)
    {
        OrderId = orderId;
        TotalValue = totalValue;
        OrderStatus = orderStatus;
    }
}
