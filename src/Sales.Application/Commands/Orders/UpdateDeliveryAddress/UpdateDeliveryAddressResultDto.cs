namespace Sales.Application.Commands.Orders.UpdateDeliveryAddress;

public sealed class UpdateDeliveryAddressResultDto
{
    public Guid OrderId { get; }
    public string DeliveryAddress { get; }
    public string OrderStatus { get; }

    public UpdateDeliveryAddressResultDto(Guid orderId, string deliveryAddress, string orderStatus)
    {
        OrderId = orderId;
        DeliveryAddress = deliveryAddress;
        OrderStatus = orderStatus;
    }
}
