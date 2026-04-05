using Sales.Domain.Common.Base;
using Sales.Domain.Common.Exceptions;
using Sales.Domain.Common.Validation;

namespace Sales.Domain.Catalog.ValueObjects;

public sealed class ProductName : ValueObject
{
    public string Value { get; }
    public ProductName(string value)
    {
        Guard.AgainstNullOrWhitespace(value, nameof(ProductName));
        Guard.Against<DomainException>(value.Trim().Length < 3, "Product name must be at least 3 characters long.");
        Guard.Against<DomainException>(value.Trim().Length > 150, "Product name cannot exceed 150 characters.");
        Value = value.Trim();
    }
    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return Value;
    }
}
