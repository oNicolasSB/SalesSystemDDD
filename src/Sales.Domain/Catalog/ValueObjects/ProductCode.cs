using Sales.Domain.Common.Base;
using Sales.Domain.Common.Exceptions;
using Sales.Domain.Common.Validation;

namespace Sales.Domain.Catalog.ValueObjects;

public sealed class ProductCode : ValueObject
{
    public string Value { get; }
    public ProductCode(string value)
    {
        Guard.AgainstNullOrWhitespace(value, nameof(ProductCode));
        Guard.Against<DomainException>(value.Trim().Length < 3, "Product code must be at least 3 characters long.");
        Value = value.Trim().ToUpper();
    }
    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return Value;
    }
}
