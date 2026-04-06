using Sales.Domain.Orders.ValueObjects;

namespace Sales.Application.Commands.Orders.UpdateDeliveryAddress;

public sealed class UpdateDeliveryAddressCommand
{
    public Guid OrderId { get; }
    public DeliveryAddress NewDeliveryAddress { get; }

    public UpdateDeliveryAddressCommand(Guid orderId, DeliveryAddress newDeliveryAddress)
    {
        OrderId = orderId;
        NewDeliveryAddress = newDeliveryAddress;
    }
}
