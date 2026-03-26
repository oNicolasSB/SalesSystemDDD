using Sales.Domain.Base;
using Sales.Domain.Common.Exceptions;
using Sales.Domain.Validation;

namespace Sales.Domain.Entities;

public sealed class OrderItem : Entity
{
    public Guid ProductId { get; private set; }
    public string ProductName { get; private set; } = string.Empty;
    public decimal UnitPrice { get; private set; }
    public int Quantity { get; private set; }
    public decimal AppliedDiscount { get; private set; }
    public decimal TotalPrice { get; private set; }

    internal OrderItem(Guid productId, string productName, decimal unitPrice, int quantity, decimal appliedDiscount)
    {
        Guard.AgainstEmptyGuid(productId, nameof(productId));
        Guard.AgainstNullOrWhitespace(productName, nameof(productName));
        Guard.Against<DomainException>(unitPrice <= 0, $"{nameof(unitPrice)} must be greater than zero.");
        Guard.Against<DomainException>(quantity <= 0, $"{nameof(quantity)} must be greater than zero.");
        Guard.Against<DomainException>(appliedDiscount < 0, $"{nameof(appliedDiscount)} cannot be negative.");

        ProductId = productId;
        ProductName = productName;
        UnitPrice = unitPrice;
        Quantity = quantity;
        AppliedDiscount = appliedDiscount;
        TotalPrice = CalculateTotalPrice();
    }
    public void ApplyDiscount(decimal discount)
    {
        Guard.Against<DomainException>(discount < 0, $"{nameof(discount)} cannot be negative.");
        Guard.Against<DomainException>(discount > (UnitPrice * Quantity), $"{nameof(discount)} cannot exceed the total price of the order item.");
        AppliedDiscount += discount;
        TotalPrice = CalculateTotalPrice();
        UpdateDate();
    }

    public void AddUnits(int additionalUnits)
    {
        Guard.Against<DomainException>(additionalUnits <= 0, $"{nameof(additionalUnits)} must be greater than zero.");
        Quantity += additionalUnits;
        TotalPrice = CalculateTotalPrice();
        UpdateDate();
    }

    public void RemoveUnits(int unitsToRemove)
    {
        Guard.Against<DomainException>(unitsToRemove <= 0, $"{nameof(unitsToRemove)} must be greater than zero.");
        Guard.Against<DomainException>(unitsToRemove >= Quantity, $"Cannot remove {unitsToRemove} units. Only {Quantity} units available.");
        Quantity -= unitsToRemove;
        TotalPrice = CalculateTotalPrice();
        UpdateDate();
    }

    public void UpdateUnitPrice(decimal newUnitPrice)
    {
        Guard.Against<DomainException>(newUnitPrice <= 0, $"{nameof(newUnitPrice)} must be greater than zero.");
        UnitPrice = newUnitPrice;
        TotalPrice = CalculateTotalPrice();
        UpdateDate();
    }

    private decimal CalculateTotalPrice()
    {
        return (UnitPrice * Quantity) - AppliedDiscount;
    }
}
