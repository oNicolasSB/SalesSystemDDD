using Sales.Application.Abstractions.Persistence;
using Sales.Domain.Common.Exceptions;
using Sales.Domain.Orders.Entities;

namespace Sales.Application.Commands.OrdersCommands.StartPayment;

public sealed class StartPaymentCommandHandler
{
    private readonly IOrderRepository _orderRepository;

    public StartPaymentCommandHandler(IOrderRepository orderRepository)
    {
        _orderRepository = orderRepository;
    }

    public async Task<StartPaymentResultDto> Handle(StartPaymentCommand command, CancellationToken cancellationToken = default)
    {
        var order = await _orderRepository.GetByIdAsync(command.OrderId, cancellationToken);

        if (order is null)
        {
            throw new DomainException($"Order with id {command.OrderId} not found.");
        }

        Payment payment = order.StartPayment(command.PaymentMethod);

        await _orderRepository.UpdateAsync(order, cancellationToken);

        return new StartPaymentResultDto
        {
            OrderId = order.Id,
            PaymentId = payment.Id,
            OrderStatus = order.OrderStatus.ToString(),
            PaymentStatus = payment.PaymentStatus.ToString()
        };
    }
}
