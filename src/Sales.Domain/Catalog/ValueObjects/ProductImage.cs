using Sales.Domain.Common.Base;
using Sales.Domain.Common.Exceptions;
using Sales.Domain.Common.Validation;

namespace Sales.Domain.Catalog.ValueObjects;

public sealed class ProductImage : ValueObject
{
    public string Url { get; }
    public int Order { get; }

    public ProductImage(string url, int order)
    {
        Guard.AgainstNullOrWhitespace(url, nameof(ProductImage));
        Guard.Against<DomainException>(order < 1, "Product image order must be at least 1.");

        Url = url;
        Order = order;
    }
    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return Url;
        yield return Order;
    }
}
