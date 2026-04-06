using Sales.Domain.Orders.ValueObjects;

namespace Sales.Application.Commands.Orders.CreateOrder;

public sealed class CreateOrderCommand
{
    public Guid ClientId { get; }
    public DeliveryAddress DeliveryAddress { get; }

    public CreateOrderCommand(Guid clientId, DeliveryAddress deliveryAddress)
    {
        ClientId = clientId;
        DeliveryAddress = deliveryAddress;
    }
}
