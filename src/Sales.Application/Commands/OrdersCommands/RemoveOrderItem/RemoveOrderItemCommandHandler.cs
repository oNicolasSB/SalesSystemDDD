using Sales.Application.Abstractions.Persistence;
using Sales.Domain.Common.Exceptions;

namespace Sales.Application.Commands.OrdersCommands.RemoveOrderItem;

public sealed class RemoveOrderItemCommandHandler
{
    private readonly IOrderRepository _orderRepository;

    public RemoveOrderItemCommandHandler(IOrderRepository orderRepository)
    {
        _orderRepository = orderRepository;
    }

    public async Task<RemoveOrderItemResultDto> Handle(RemoveOrderItemCommand command, CancellationToken cancellationToken = default)
    {
        var order = await _orderRepository.GetByIdAsync(command.OrderId, cancellationToken);
        if (order is null)
            throw new DomainException("Order not found.");

        order.RemoveOrderItem(command.ItemId);

        await _orderRepository.UpdateAsync(order, cancellationToken);


        return new RemoveOrderItemResultDto(
            order.Id,
            order.TotalValue,
            order.OrderStatus.ToString()
            );
    }

}
