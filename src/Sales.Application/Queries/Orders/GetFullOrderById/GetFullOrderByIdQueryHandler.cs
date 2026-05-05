using Sales.Application.Abstractions.Persistence;
using Sales.Application.Queries.Orders.Dtos;

namespace Sales.Application.Queries.Orders.GetFullOrderById;

public sealed class GetFullOrderByIdQueryHandler
{
    private readonly IOrderQueryRepository _queryRepository;

    public GetFullOrderByIdQueryHandler(IOrderQueryRepository queryRepository)
    {
        _queryRepository = queryRepository;
    }

    public async Task<OrderFullDto?> HandleAsync(
        GetFullOrderByIdQuery query,
        CancellationToken cancellationToken = default
    )
    {
        return await _queryRepository.GetFullOrderByIdAsync(query.OrderId, cancellationToken);
    }
}
