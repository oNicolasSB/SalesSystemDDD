using Sales.Application.Abstractions.Persistence;
using Sales.Domain.Common.Exceptions;
using Sales.Domain.Orders.ValueObjects;

namespace Sales.Application.Commands.OrdersCommands.CancelOrder;

public sealed class CancelOrderCommandHandler
{
    private readonly IOrderRepository _orderRepository;

    public CancelOrderCommandHandler(IOrderRepository orderRepository)
    {
        _orderRepository = orderRepository;
    }

    public async Task<CancelOrderResultDto> HandleAsync(CancelOrderCommand command, CancellationToken cancellationToken = default)
    {
        var order = await _orderRepository.GetByIdAsync(command.OrderId, cancellationToken);

        if (order is null)
        {
            throw new DomainException($"Order not found.");
        }

        var reason = new CancelReason(command.ReasonCode);
        order.CancelOrder(reason);

        await _orderRepository.UpdateAsync(order, cancellationToken);

        return new CancelOrderResultDto
        {
            OrderId = order.Id,
            OrderStatus = order.OrderStatus.ToString()
        };
    }
}
