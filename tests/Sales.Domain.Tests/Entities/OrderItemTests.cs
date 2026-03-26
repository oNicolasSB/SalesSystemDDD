using Sales.Domain.Common.Exceptions;
using Sales.Domain.Entities;

namespace Sales.Domain.Tests.Entities;

public class OrderItemTests
{
    //aux method
    private static OrderItem CreateValidOrderItem(string name = "Product Name", decimal unitPrice = 10.0m, int quantity = 2, decimal appliedDiscount = 0m)
    {
        return new OrderItem(Guid.NewGuid(), name, unitPrice, quantity, appliedDiscount);
    }

    [Fact(DisplayName = "Should create OrderItem with valid parameters")]
    public void Create_ShouldSucceed_WhenParametersAreValid()
    {
        // Arrange
        Guid productId = Guid.NewGuid();
        string productName = "Product Name";
        decimal unitPrice = 10.0m;
        int quantity = 2;
        decimal appliedDiscount = 0m;

        // Act
        OrderItem orderItem = new OrderItem(productId, productName, unitPrice, quantity, appliedDiscount);

        // Assert
        orderItem.ProductId.Should().Be(productId);
        orderItem.ProductName.Should().Be(productName);
        orderItem.UnitPrice.Should().Be(unitPrice);
        orderItem.Quantity.Should().Be(quantity);
        orderItem.AppliedDiscount.Should().Be(appliedDiscount);
        orderItem.TotalPrice.Should().Be(unitPrice * quantity - appliedDiscount);
    }

    [Theory(DisplayName = "Should throw DomainException when parameters are invalid")]
    [InlineData("00000000-0000-0000-0000-000000000000", "Product Name", 10.0, 2, 0, "'productId' cannot be an empty GUID.")]
    [InlineData("00000000-0000-0000-0000-000000000001", "", 10.0, 2, 0, "'productName' cannot be null or whitespace.")]
    [InlineData("00000000-0000-0000-0000-000000000001", "Product Name", 0, 2, 0, "unitPrice must be greater than zero.")]
    [InlineData("00000000-0000-0000-0000-000000000001", "Product Name", 10.0, 0, 0, "quantity must be greater than zero.")]
    [InlineData("00000000-0000-0000-0000-000000000001", "Product Name", 10.0, 2, -1, "appliedDiscount cannot be negative.")]
    public void Create_ShouldThrowDomainException_WhenParametersAreInvalid(string productIdStr, string productName, decimal unitPrice, int quantity, decimal appliedDiscount, string expectedMessage)
    {
        // Arrange
        Guid productId = Guid.Parse(productIdStr);

        // Act
        Action act = () => new OrderItem(productId, productName, unitPrice, quantity, appliedDiscount);

        // Assert
        act.Should().Throw<DomainException>().WithMessage(expectedMessage);
    }

    [Fact(DisplayName = "Should apply discount correctly")]
    public void ApplyDiscount_ShouldUpdateTotalPrice()
    {
        // Arrange
        var orderItem = CreateValidOrderItem(unitPrice: 20.0m, quantity: 3);
        decimal discount = 5.0m;

        // Act
        orderItem.ApplyDiscount(discount);

        // Assert
        orderItem.AppliedDiscount.Should().Be(discount);
        orderItem.TotalPrice.Should().Be(orderItem.UnitPrice * orderItem.Quantity - discount);
        orderItem.UpdatedAt.Should().NotBeNull();
    }

    [Theory(DisplayName = "Should throw DomainException when applying invalid discount")]
    [InlineData(-1.0, "discount cannot be negative.")]
    [InlineData(10000.0, "discount cannot exceed the total price of the order item.")]
    public void ApplyDiscount_ShouldThrowDomainException_WhenDiscountIsInvalid(decimal invalidDiscount, string expectedMessage)
    {        // Arrange
        var orderItem = CreateValidOrderItem(unitPrice: 20.0m, quantity: 3);

        // Act
        Action act = () => orderItem.ApplyDiscount(invalidDiscount);

        // Assert
        act.Should().Throw<DomainException>().WithMessage(expectedMessage);
    }

    [Fact(DisplayName = "Should add units correctly")]
    public void AddUnits_ShouldUpdateQuantityAndTotalPrice()
    {
        // Arrange
        var orderItem = CreateValidOrderItem(unitPrice: 20.0m, quantity: 3);
        int additionalUnits = 2;

        // Act
        orderItem.AddUnits(additionalUnits);

        // Assert
        orderItem.Quantity.Should().Be(5);
        orderItem.TotalPrice.Should().Be(orderItem.UnitPrice * orderItem.Quantity - orderItem.AppliedDiscount);
        orderItem.UpdatedAt.Should().NotBeNull();
    }

    [Fact(DisplayName = "Should throw DomainException when adding invalid units")]
    public void AddUnits_ShouldThrowDomainException_WhenUnitsAreInvalid()
    {
        // Arrange
        var orderItem = CreateValidOrderItem(unitPrice: 20.0m, quantity: 3);
        int invalidUnits = -1;

        // Act
        Action act = () => orderItem.AddUnits(invalidUnits);

        // Assert
        act.Should().Throw<DomainException>().WithMessage("additionalUnits must be greater than zero.");
    }

    [Fact(DisplayName = "Should remove units correctly")]
    public void RemoveUnits_ShouldUpdateQuantityAndTotalPrice()
    {
        // Arrange
        var orderItem = CreateValidOrderItem(unitPrice: 20.0m, quantity: 5);
        int unitsToRemove = 2;

        // Act
        orderItem.RemoveUnits(unitsToRemove);

        // Assert
        orderItem.Quantity.Should().Be(3);
        orderItem.TotalPrice.Should().Be(orderItem.UnitPrice * orderItem.Quantity - orderItem.AppliedDiscount);
        orderItem.UpdatedAt.Should().NotBeNull();
    }

    [Fact(DisplayName = "Should throw DomainException when removing invalid units")]
    public void RemoveUnits_ShouldThrowDomainException_WhenUnitsToRemoveAreInvalid()
    {
        // Arrange
        var orderItem = CreateValidOrderItem(unitPrice: 20.0m, quantity: 5);
        int invalidUnitsToRemove = -1;

        // Act
        Action act = () => orderItem.RemoveUnits(invalidUnitsToRemove);

        // Assert
        act.Should().Throw<DomainException>().WithMessage("unitsToRemove must be greater than zero.");
    }

    [Fact(DisplayName = "Should throw DomainException when removing units and reach zero or negative quantity")]
    public void RemoveUnits_ShouldThrowDomainException_WhenRemovingUnitsExceedsQuantity()
    {
        // Arrange
        var orderItem = CreateValidOrderItem(unitPrice: 20.0m, quantity: 5);
        int unitsToRemove = 5;

        // Act
        Action act = () => orderItem.RemoveUnits(unitsToRemove);

        // Assert
        act.Should().Throw<DomainException>().WithMessage($"Cannot remove {unitsToRemove} units. Only {orderItem.Quantity} units available.");
    }

    [Fact(DisplayName = "Should update unit price correctly")]
    public void UpdateUnitPrice_ShouldUpdateUnitPriceAndTotalPrice()
    {
        // Arrange
        var orderItem = CreateValidOrderItem(unitPrice: 20.0m, quantity: 3);
        decimal newUnitPrice = 25.0m;

        // Act
        orderItem.UpdateUnitPrice(newUnitPrice);

        // Assert
        orderItem.UnitPrice.Should().Be(newUnitPrice);
        orderItem.TotalPrice.Should().Be(orderItem.UnitPrice * orderItem.Quantity - orderItem.AppliedDiscount);
        orderItem.UpdatedAt.Should().NotBeNull();
    }
}
