using Sales.Application.Abstractions.Persistence;
using Sales.Domain.Common.Exceptions;

namespace Sales.Application.Commands.Orders.MarkAsShipped;

public sealed class MarkAsShippedCommandHandler
{
    private readonly IOrderRepository _orderRepository;

    public MarkAsShippedCommandHandler(IOrderRepository orderRepository)
    {
        _orderRepository = orderRepository;
    }

    public async Task<MarkAsShippedResultDto> Handle(MarkAsShippedCommand command, CancellationToken cancellationToken)
    {
        var order = await _orderRepository.GetByIdAsync(command.OrderId, cancellationToken);

        if (order is null)
        {
            throw new DomainException($"Order not found.");
        }

        order.MarkAsShipped();

        await _orderRepository.UpdateAsync(order, cancellationToken);

        return new MarkAsShippedResultDto
        {
            OrderId = order.Id,
            OrderStatus = order.OrderStatus.ToString()
        };
    }
}
