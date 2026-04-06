using Sales.Application.Abstractions.Persistence;
using Sales.Domain.Orders.Entities;

namespace Sales.Application.Commands.Orders.CreateOrder;

public sealed class CreateOrderCommandHandler
{
    private readonly IOrderRepository _orderRepository;

    public CreateOrderCommandHandler(IOrderRepository orderRepository)
    {
        _orderRepository = orderRepository;
    }

    public async Task<CreateOrderResultDto> Handle(CreateOrderCommand command, CancellationToken cancellationToken = default)
    {
        var order = Order.Create(command.ClientId, command.DeliveryAddress);

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
