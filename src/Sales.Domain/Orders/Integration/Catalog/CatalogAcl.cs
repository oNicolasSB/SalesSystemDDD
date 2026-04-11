using Sales.Domain.Common.Exceptions;
using Sales.Domain.Common.Validation;

namespace Sales.Domain.Orders.Integration.Catalog;

public sealed class CatalogAcl
{
    private readonly ICatalogGateway _catalogGateway;

    public CatalogAcl(ICatalogGateway catalogGateway)
    {
        Guard.AgainstNull(catalogGateway, nameof(catalogGateway));
        _catalogGateway = catalogGateway;
    }

    public async Task<ProductSnapshot> GetProductSnapshotAsync(Guid productId, CancellationToken cancellationToken = default)
    {
        var productDto = await _catalogGateway.GetProductByIdAsync(productId, cancellationToken)
            ?? throw new DomainException($"Product not found on the catalog.");

        return new ProductSnapshot(productDto.Id, productDto.Name, productDto.Price);
    }

    public async Task ValidateStockAsync(Guid productId, int quantity, CancellationToken cancellationToken = default)
    {
        var hasSufficientStock = await _catalogGateway.HasSufficientStockAsync(productId, quantity, cancellationToken);
        if (!hasSufficientStock)
        {
            throw new DomainException($"Insufficient stock for product with id {productId}.");
        }
    }
}
