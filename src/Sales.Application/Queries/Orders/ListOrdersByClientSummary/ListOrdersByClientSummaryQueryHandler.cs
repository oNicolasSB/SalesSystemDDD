using Sales.Application.Abstractions.Persistence;
using Sales.Application.Queries.Orders.Dtos;

namespace Sales.Application.Queries.Orders.ListOrdersByClientSummary;

public sealed class ListOrdersByClientSummaryQueryHandler
{
    private readonly IOrderQueryRepository _queryRepository;

    public ListOrdersByClientSummaryQueryHandler(IOrderQueryRepository queryRepository)
    {
        _queryRepository = queryRepository;
    }

    public async Task<IReadOnlyList<OrderSummaryDto>> HandleAsync(
        ListOrdersByClientSummaryQuery query,
        CancellationToken cancellationToken = default
    ) => await _queryRepository.ListSummaryByClientAsync(query.ClientId, cancellationToken);

}
