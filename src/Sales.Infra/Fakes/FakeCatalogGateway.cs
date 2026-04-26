using Sales.Domain.Orders.Integration.Catalog;

namespace Sales.Infra.Fakes;

public sealed class FakeCatalogGateway : ICatalogGateway
{
    private static readonly Dictionary<Guid, ProductDto> _products = new()
    {
        [Guid.Parse("11111111-1111-1111-1111-111111111111")] = new ProductDto(new Guid("11111111-1111-1111-1111-111111111111"), "Product 1", 10.0m),
        [Guid.Parse("22222222-2222-2222-2222-222222222222")] = new ProductDto(new Guid("22222222-2222-2222-2222-222222222222"), "Product 2", 20.0m),
        [Guid.Parse("33333333-3333-3333-3333-333333333333")] = new ProductDto(new Guid("33333333-3333-3333-3333-333333333333"), "Product 3", 30.0m),
        [Guid.Parse("44444444-4444-4444-4444-444444444444")] = new ProductDto(new Guid("44444444-4444-4444-4444-444444444444"), "Product 4", 40.0m),
        [Guid.Parse("55555555-5555-5555-5555-555555555555")] = new ProductDto(new Guid("55555555-5555-5555-5555-555555555555"), "Product 5", 50.0m)
    };
    public Task<ProductDto?> GetProductByIdAsync(Guid productId, CancellationToken cancellationToken = default)
    {
        _products.TryGetValue(productId, out var product);
        return Task.FromResult(product);
    }
}
