using Sales.Application.Abstractions.Persistence;
using Sales.Domain.Common.Exceptions;
using Sales.Domain.Orders.Integration.Catalog;

namespace Sales.Application.Commands.OrdersCommands.AddOrderItem;

public class AddOrderItemCommandHandler
{
    private readonly IOrderRepository _orderRepository;
    private readonly ICatalogGateway _catalogGateway;
    private readonly CatalogAcl _catalogAcl;

    public AddOrderItemCommandHandler(IOrderRepository orderRepository, ICatalogGateway catalogGateway, CatalogAcl catalogAcl)
    {
        _orderRepository = orderRepository;
        _catalogGateway = catalogGateway;
        _catalogAcl = catalogAcl;
    }

    public async Task<AddOrderItemResultDto> HandleAsync(AddOrderItemCommand command, CancellationToken cancellationToken = default)
    {
        var order = await _orderRepository.GetByIdAsync(command.OrderId, cancellationToken);

        if (order is null)
        {
            throw new DomainException($"Order not found.");
        }

        ProductDto? productDto = await _catalogGateway.GetProductByIdAsync(command.ProductId, cancellationToken);

        if(productDto is null) throw new InvalidOperationException($"Product not found in catalog.");

        var (productName, unitPrice) = _catalogAcl.TranslateProduct(productDto);

        order.AddOrderItem(command.ProductId, productName, unitPrice, command.Quantity);

        await _orderRepository.UpdateAsync(order, cancellationToken);

        return new AddOrderItemResultDto(
            order.Id,
            order.TotalValue,
            order.OrderStatus.ToString()
        );
    }
}
