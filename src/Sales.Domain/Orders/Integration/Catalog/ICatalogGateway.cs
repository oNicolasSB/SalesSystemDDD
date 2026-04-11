namespace Sales.Domain.Orders.Integration.Catalog;

public interface ICatalogGateway
{
    Task<ProductDto?> GetProductByIdAsync(Guid productId, CancellationToken cancellationToken = default);
    Task<bool> HasSufficientStockAsync(Guid productId, int quantity, CancellationToken cancellationToken = default);
}
