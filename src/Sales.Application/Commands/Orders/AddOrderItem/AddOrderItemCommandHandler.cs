using Sales.Application.Abstractions.Persistence;
using Sales.Domain.Common.Exceptions;

namespace Sales.Application.Commands.Orders.AddOrderItem;

public class AddOrderItemCommandHandler
{
    private readonly IOrderRepository _orderRepository;

    public AddOrderItemCommandHandler(IOrderRepository orderRepository)
    {
        _orderRepository = orderRepository;
    }

    public async Task<AddOrderItemResultDto> Handle(AddOrderItemCommand command, CancellationToken cancellationToken = default)
    {
        var order = await _orderRepository.GetByIdAsync(command.OrderId, cancellationToken);

        if (order is null)
        {
            throw new DomainException($"Order not found.");
        }

        order.AddOrderItem(command.ProductId, command.ProductName, command.UnitPrice, command.Quantity);

        await _orderRepository.UpdateAsync(order, cancellationToken);

        return new AddOrderItemResultDto(
            order.Id,
            order.TotalValue,
            order.OrderStatus.ToString()
        );
    }
}
