using Sales.Application.Abstractions.Persistence;
using Sales.Domain.Orders.Entities;

namespace Sales.Infra.Fakes;

public sealed class FakeOrderRepository : IOrderRepository
{
    private readonly Dictionary<Guid, Order> _orders = [];
    private readonly SemaphoreSlim _lock = new(1, 1);
    public async Task AddAsync(Order order, CancellationToken cancellationToken = default)
    {
        await _lock.WaitAsync(cancellationToken);
        try
        {
            _orders[order.Id] = order;
        }
        finally
        {
            _lock.Release();
        }
    }

    public async Task<Order?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        await _lock.WaitAsync(cancellationToken);
        try
        {
            _orders.TryGetValue(id, out var order);
            return order;
        }
        finally
        {
            _lock.Release();
        }
    }

    public async Task UpdateAsync(Order order, CancellationToken cancellationToken = default)
    {
        await _lock.WaitAsync(cancellationToken);
        try
        {
            if(!_orders.ContainsKey(order.Id))
            {
                throw new KeyNotFoundException($"Order with id {order.Id} not found.");
            }
            _orders[order.Id] = order;
        }
        finally
        {
            _lock.Release();
        }
    }

    public async Task<IReadOnlyList<Order>> ListAllAsync(CancellationToken cancellationToken = default)
    {
        await _lock.WaitAsync(cancellationToken);
        try
        {
            return _orders.Values.ToList().AsReadOnly();
        }
        finally
        {
            _lock.Release();
        }
    }
}
