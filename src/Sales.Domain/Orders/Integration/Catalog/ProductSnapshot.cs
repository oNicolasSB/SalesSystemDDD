using Sales.Domain.Common.Base;
using Sales.Domain.Common.Exceptions;
using Sales.Domain.Common.Validation;

namespace Sales.Domain.Orders.Integration.Catalog;

public sealed class ProductSnapshot : ValueObject
{

    public Guid ProductId { get; }
    public string ProductName { get; }
    public decimal UnitPrice { get; }

    public ProductSnapshot(Guid productId, string productName, decimal unitPrice)
    {
        Guard.AgainstEmptyGuid(productId, nameof(productId));
        Guard.AgainstNullOrWhitespace(productName, nameof(productName));
        Guard.Against<DomainException>(unitPrice <= 0, nameof(unitPrice));

        ProductId = productId;
        ProductName = productName;
        UnitPrice = unitPrice;
    }

    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return ProductId;
        yield return ProductName;
        yield return UnitPrice;
    }
}
