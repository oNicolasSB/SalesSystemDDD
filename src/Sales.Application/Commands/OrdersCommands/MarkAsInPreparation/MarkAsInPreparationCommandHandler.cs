using Sales.Application.Abstractions.Persistence;

namespace Sales.Application.Commands.OrdersCommands.MarkAsInPreparation;

public sealed class MarkAsInPreparationCommandHandler
{
    private readonly IOrderRepository _orderRepository;

    public MarkAsInPreparationCommandHandler(IOrderRepository orderRepository)
    {
        _orderRepository = orderRepository;
    }

    public async Task<MarkAsInPreparationResultDto> HandleAsync(MarkAsInPreparationCommand command, CancellationToken cancellationToken = default)
    {
        var order = await _orderRepository.GetByIdAsync(command.OrderId, cancellationToken);
        if (order is null)
        {
            throw new KeyNotFoundException($"Order not found.");
        }

        order.MarkAsInPreparation();
        await _orderRepository.UpdateAsync(order, cancellationToken);

        return new MarkAsInPreparationResultDto
        {
            OrderId = order.Id,
            OrderStatus = order.OrderStatus.ToString()
        };
    }
}
