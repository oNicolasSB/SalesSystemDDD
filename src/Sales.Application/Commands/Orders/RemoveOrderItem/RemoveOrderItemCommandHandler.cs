using Sales.Application.Abstractions.Persistence;

namespace Sales.Application.Commands.Orders.RemoveOrderItem;

public sealed class RemoveOrderItemCommandHandler
{
    private readonly IOrderRepository _orderRepository;

    public RemoveOrderItemCommandHandler(IOrderRepository orderRepository)
    {
        _orderRepository = orderRepository;
    }

    public async Task<RemoveOrderItemResultDto> Handle(RemoveOrderItemCommand command)
    {
        var order = await _orderRepository.GetByIdAsync(command.OrderId);
        if (order is null)
            throw new InvalidOperationException("Order not found.");

        order.RemoveOrderItem(command.ItemId);

        await _orderRepository.UpdateAsync(order);
        

        return new RemoveOrderItemResultDto(
            order.Id,
            order.TotalValue,
            order.OrderStatus.ToString()
            );
    }

}
