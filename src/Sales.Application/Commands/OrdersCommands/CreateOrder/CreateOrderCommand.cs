using Sales.Domain.Orders.ValueObjects;

namespace Sales.Application.Commands.OrdersCommands.CreateOrder;

public sealed class CreateOrderCommand
{
    public Guid ClientId { get; }
    public Guid AddressId { get; }

    public CreateOrderCommand(Guid clientId, Guid addressId)
    {
        ClientId = clientId;
        AddressId = addressId;
    }
}
