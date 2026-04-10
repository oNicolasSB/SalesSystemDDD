namespace Sales.Application.Commands.OrdersCommands.CreateOrder;

public class CreateOrderResultDto
{
    public Guid OrderId { get; }
    public string OrderNumber { get; }
    public DateTime CreatedAt { get; }
    public decimal TotalValue { get; }
    public string Status { get; }

    public CreateOrderResultDto(Guid orderId, string orderNumber, DateTime createdAt, decimal totalValue, string status)
    {
        OrderId = orderId;
        OrderNumber = orderNumber;
        CreatedAt = createdAt;
        TotalValue = totalValue;
        Status = status;
    }
}
