using Sales.Application.Abstractions.Persistence;
using Sales.Application.Queries.Orders.Dtos;

namespace Sales.Application.Queries.Orders.ListOrdersSummary;

public sealed class ListOrdersSummaryQueryHandler
{
    private readonly IOrderQueryRepository _queryRepository;

    public ListOrdersSummaryQueryHandler(IOrderQueryRepository queryRepository)
    {
        _queryRepository = queryRepository;
    }


    public async Task<IReadOnlyList<OrderSummaryDto>> HandleAsync(
        ListOrdersSummaryQuery query,
        CancellationToken cancellationToken = default
    ) => await _queryRepository.ListSummaryAsync(cancellationToken);
}
