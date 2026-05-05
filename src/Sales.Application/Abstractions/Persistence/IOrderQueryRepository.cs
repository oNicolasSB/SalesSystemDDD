using Sales.Application.Queries.Orders.Dtos;
using Sales.Domain.Orders.Enums;

namespace Sales.Application.Abstractions.Persistence;

public interface IOrderQueryRepository
{
    Task<IReadOnlyList<OrderSummaryDto>> ListSummaryAsync(CancellationToken cancellationToken = default);
    Task<IReadOnlyList<OrderSummaryDto>> ListSummaryByClientAsync(Guid clientId, CancellationToken cancellationToken = default);
    Task<IReadOnlyList<PaymentByStatusDto>> ListPaymentsByStatusAsync(PaymentStatus status,  CancellationToken cancellationToken = default);
    Task<OrderFullDto?> GetFullOrderByIdAsync(Guid orderId, CancellationToken cancellationToken = default);
}
