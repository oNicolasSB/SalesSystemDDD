using Microsoft.EntityFrameworkCore;
using Sales.Application.Abstractions.Persistence;
using Sales.Application.Queries.Orders.Dtos;
using Sales.Domain.Orders.Entities;
using Sales.Domain.Orders.Enums;
using Sales.Infra.Persistence.Context;

namespace Sales.Infra.Repositories;

public sealed class OrderQueryRepository : IOrderQueryRepository
{
    private readonly SalesDbContext _context;

    public OrderQueryRepository(SalesDbContext context)
    {
        _context = context;
    }

    public async Task<OrderFullDto?> GetFullOrderByIdAsync(Guid orderId, CancellationToken cancellationToken = default)
    {
        return await _context.Orders.AsNoTracking()
            .Where(o => o.Id == orderId)
            .Select(o => new OrderFullDto
            {
                Id = o.Id,
                OrderNumber = o.OrderNumber,
                ClientId = o.ClientId,
                TotalValue = o.TotalValue,
                OrderStatus = o.OrderStatus.ToString(),
                CreatedAt = o.CreatedAt,
                Address = new DeliveryAddressDto
                {
                    Cep = o.DeliveryAddress.Cep,
                    Street = o.DeliveryAddress.Street,
                    Number = o.DeliveryAddress.Number,
                    Complement = o.DeliveryAddress.Complement ?? string.Empty,
                    Neighborhood = o.DeliveryAddress.Neighborhood,
                    City = o.DeliveryAddress.City,
                    State = o.DeliveryAddress.State,
                    Country = o.DeliveryAddress.Country

                },
                Items = o.OrderItems.Select(i => new ItemSummaryDto
                {
                    ProductId = i.ProductId,
                    ProductName = i.ProductName,
                    UnitPrice = i.UnitPrice,
                    Quantity = i.Quantity,
                    TotalValue = i.TotalPrice
                }).ToList(),
                Payments = o.Payments.Select(p => new PaymentDto
                {
                    PaymentId = p.Id,
                    PaymentMethod = p.PaymentMethod.ToString(),
                    PaymentStatus = p.PaymentStatus.ToString(),
                    Value = p.Value,
                    TransactionCode = p.TransactionCode,
                    PaidAt = p.PaidAt

                }).ToList()
            }).FirstOrDefaultAsync(cancellationToken);
    }

    public async Task<IReadOnlyList<PaymentByStatusDto>> ListPaymentsByStatusAsync(PaymentStatus status, CancellationToken cancellationToken = default)
    {
        return await _context.Set<Payment>().AsNoTracking()
            .Where(p => p.PaymentStatus == status)
            .Join(
                _context.Orders,
                pg => pg.OrderId,
                p => p.Id, (pg, p) => new PaymentByStatusDto
                {
                    PaymentId = pg.Id,
                    OrderNumber = p.OrderNumber,
                    ClientId = p.ClientId,
                    TotalValue = p.TotalValue,
                    PaymentStatus = pg.PaymentStatus.ToString(),
                    PaymentMethod = pg.PaymentMethod.ToString(),
                    TransactionCode = pg.TransactionCode,
                    PaidAt = pg.PaidAt
                }
            )
            .OrderBy(dto => dto.OrderNumber).ToListAsync();
    }

    public async Task<IReadOnlyList<OrderSummaryDto>> ListSummaryAsync(
        CancellationToken cancellationToken = default
        )
    {
        return await _context.Orders
            .AsNoTracking()
            .Select(o => new OrderSummaryDto
            {
                Id = o.Id,
                OrderNumber = o.OrderNumber,
                ClientId = o.ClientId,
                TotalValue = o.TotalValue,
                OrderStatus = o.OrderStatus.ToString(),
                CreatedAt = o.CreatedAt,
                TotalItems = o.OrderItems.Count,
                TotalPayments = o.Payments.Count
            })
            .OrderByDescending(o => o.CreatedAt)
            .ToListAsync(cancellationToken);
    }

    public async Task<IReadOnlyList<OrderSummaryDto>> ListSummaryByClientAsync(
        Guid clientId,
        CancellationToken cancellationToken = default
        )
    {
        return await _context.Orders
            .AsNoTracking()
            .Where(p => p.ClientId == clientId)
            .Select(o => new OrderSummaryDto
            {
                Id = o.Id,
                OrderNumber = o.OrderNumber,
                ClientId = o.ClientId,
                TotalValue = o.TotalValue,
                OrderStatus = o.OrderStatus.ToString(),
                CreatedAt = o.CreatedAt,
                TotalItems = o.OrderItems.Count,
                TotalPayments = o.Payments.Count
            })
            .OrderByDescending(o => o.CreatedAt)
            .ToListAsync(cancellationToken);
    }
}
