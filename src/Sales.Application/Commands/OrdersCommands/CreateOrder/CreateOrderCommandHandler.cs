using Sales.Application.Abstractions.Persistence;
using Sales.Domain.Orders.Entities;
using Sales.Domain.Orders.Integration.Clients;
using Sales.Domain.Orders.ValueObjects;

namespace Sales.Application.Commands.OrdersCommands.CreateOrder;

public sealed class CreateOrderCommandHandler
{
    private readonly IOrderRepository _orderRepository;
    private readonly IClientGateway _clientGateway;
    private readonly ClientAcl _clientAcl;

    public CreateOrderCommandHandler(
        IOrderRepository orderRepository,
        IClientGateway clientGateway,
        ClientAcl clientAcl)
    {
        _orderRepository = orderRepository;
        _clientGateway = clientGateway;
        _clientAcl = clientAcl;
    }

    public async Task<CreateOrderResultDto> Handle(CreateOrderCommand command, CancellationToken cancellationToken = default)
    {
        AddressDto? addressDto = await _clientGateway.GetAddressAsync(command.ClientId, command.AddressId, cancellationToken);

        if (addressDto is null)
        {
            throw new InvalidOperationException("Address not found");
        }

        DeliveryAddress deliveryAddress = _clientAcl.TranslateAddress(addressDto);

        var order = Order.Create(command.ClientId, deliveryAddress);

        await _orderRepository.AddAsync(order, cancellationToken);

        return new CreateOrderResultDto(
            order.Id,
            order.OrderNumber,
            order.CreatedAt,
            order.TotalValue,
            order.OrderStatus.ToString()
        );
    }
}
