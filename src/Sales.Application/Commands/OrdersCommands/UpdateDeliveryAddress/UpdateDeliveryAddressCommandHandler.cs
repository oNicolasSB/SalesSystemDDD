using Sales.Application.Abstractions.Persistence;
using Sales.Domain.Common.Exceptions;

namespace Sales.Application.Commands.OrdersCommands.UpdateDeliveryAddress;

public class UpdateDeliveryAddressCommandHandler
{
    private readonly IOrderRepository _orderRepository;

    public UpdateDeliveryAddressCommandHandler(IOrderRepository orderRepository)
    {
        _orderRepository = orderRepository;
    }


    public async Task<UpdateDeliveryAddressResultDto> Handle(UpdateDeliveryAddressCommand command, CancellationToken cancellationToken = default)
    {
        var order = await _orderRepository.GetByIdAsync(command.OrderId, cancellationToken);
        if (order is null)
            throw new DomainException("Order not found.");

        order.UpdateDeliveryAddress(command.NewDeliveryAddress);

        await _orderRepository.UpdateAsync(order, cancellationToken);

        return new UpdateDeliveryAddressResultDto(
            order.Id,
            order.DeliveryAddress.ToString()!,
            order.OrderStatus.ToString()
            );
    }
}
