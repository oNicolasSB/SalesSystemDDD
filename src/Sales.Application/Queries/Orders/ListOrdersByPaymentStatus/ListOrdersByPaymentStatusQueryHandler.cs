using Sales.Application.Abstractions.Persistence;
using Sales.Application.Queries.Orders.Dtos;

namespace Sales.Application.Queries.Orders.ListOrdersByPaymentStatus;

public class ListOrdersByPaymentStatusQueryHandler
{
    private readonly IOrderQueryRepository _queryRepository;

    public ListOrdersByPaymentStatusQueryHandler(IOrderQueryRepository queryRepository)
    {
        _queryRepository = queryRepository;
    }

    public async Task<IReadOnlyList<PaymentByStatusDto>> HandleAsync(
        ListOrdersByPaymentStatusQuery query,
        CancellationToken cancellationToken = default
    )
    {
        return await _queryRepository.ListPaymentsByStatusAsync(query.Status, cancellationToken);
    }
}
