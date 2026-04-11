namespace Sales.Domain.Orders.Integration.Catalog;

public interface ICatalogGateway
{
    Task<ProductDto?> GetProductByIdAsync(Guid productId, CancellationToken cancellationToken = default);
}
