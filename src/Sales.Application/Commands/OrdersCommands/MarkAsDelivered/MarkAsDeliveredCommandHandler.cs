using Sales.Application.Abstractions.Persistence;
using Sales.Domain.Common.Exceptions;

namespace Sales.Application.Commands.OrdersCommands.MarkAsDelivered;

public sealed class MarkAsDeliveredCommandHandler
{
    private readonly IOrderRepository _orderRepository;

    public MarkAsDeliveredCommandHandler(IOrderRepository orderRepository)
    {
        _orderRepository = orderRepository;
    }

    public async Task<MarkAsShippedResultDto> Handle(MarkAsDeliveredCommand command, CancellationToken cancellationToken)
    {
        var order = await _orderRepository.GetByIdAsync(command.OrderId, cancellationToken);

        if (order is null)
        {
            throw new DomainException($"Order not found.");
        }

        order.MarkAsDelivered();

        await _orderRepository.UpdateAsync(order, cancellationToken);

        return new MarkAsShippedResultDto
        {
            OrderId = order.Id,
            OrderStatus = order.OrderStatus.ToString()
        };
    }
}
