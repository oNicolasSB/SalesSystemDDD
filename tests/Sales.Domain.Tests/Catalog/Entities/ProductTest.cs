using Sales.Domain.Catalog.Entities;
using Sales.Domain.Catalog.Enums;
using Sales.Domain.Catalog.Events;
using Sales.Domain.Catalog.ValueObjects;
using Sales.Domain.Common.Exceptions;

namespace Sales.Domain.Tests.Catalog.Entities;

public class ProductTest
{
    private static Product CreateValidProduct(
        string name = "Test Product",
        string code = "TEST123",
        decimal price = 10.0m,
        int stockQuantity = 100,
        string? description = null)
    {
        return new Product(
            new ProductName(name),
            new ProductCode(code),
            new ProductPrice(price),
            Guid.NewGuid(),
            stockQuantity,
            description
        );
    }

    [Fact]
    public void CreateValidProduct_ShouldReturnProduct()
    {
        // Arrange & Act
        var product = CreateValidProduct();

        // Assert
        Assert.NotNull(product);
    }

    [Fact]
    public void CreateProduct_WithValidParameters_ShouldCreateProduct()
    {
        // Arrange
        var name = "Test Product";
        var code = "TEST123";
        var price = 10.0m;
        var stockQuantity = 100;
        var description = "This is a test product.";

        // Act
        var product = CreateValidProduct(name, code, price, stockQuantity, description);

        // Assert
        product.Should().NotBeNull();
        product.Name.Value.Should().Be(name);
        product.Code.Value.Should().Be(code);
        product.Price.Value.Should().Be(price);
        product.Description.Should().Be(description);
        product.CategoryId.Should().NotBeEmpty();
        product.Status.Should().Be(ProductStatus.Active);
        product.StockQuantity.Should().Be(stockQuantity);
    }

    [Fact]
    public void UpdateName_WithValidName_ShouldUpdateProductName()
    {
        // Arrange
        var product = CreateValidProduct();
        var newName = "Updated Product Name";

        // Act
        product.UpdateName(new ProductName(newName));

        // Assert
        product.Name.Value.Should().Be(newName);
    }

    [Fact]
    public void UpdatePrice_WithValidPrice_ShouldUpdateProductPrice()
    {
        // Arrange
        var product = CreateValidProduct(price: 10.0m);
        var newPrice = 20.0m;

        // Act
        product.UpdatePrice(new ProductPrice(newPrice));

        // Assert
        product.Price.Value.Should().Be(newPrice);
        product.DomainEvents.Should().ContainSingle(e => e is ProductPriceChangedEvent);
        product.DomainEvents.OfType<ProductPriceChangedEvent>().First().NewPrice.Should().Be(newPrice);
        product.DomainEvents.OfType<ProductPriceChangedEvent>().First().OldPrice.Should().Be(10.0m);
    }

    [Fact]
    public void UpdateStock_WithValidQuantity_ShouldUpdateStockQuantity()
    {
        // Arrange
        var product = CreateValidProduct(stockQuantity: 100);
        var quantityToAdd = 50;
        var reason = "Restocking";

        // Act
        product.UpdateStock(quantityToAdd, reason);

        // Assert
        product.StockQuantity.Should().Be(150);
        product.DomainEvents.Should().ContainSingle(e => e is StockUpdatedEvent);
        product.DomainEvents.OfType<StockUpdatedEvent>().First().Quantity.Should().Be(150);
        product.DomainEvents.OfType<StockUpdatedEvent>().First().Reason.Should().Be(reason);
    }

    [Fact]
    public void UpdateStock_WithNegativeQuantity_ShouldThrowDomainException()
    {
        // Arrange
        var product = CreateValidProduct(stockQuantity: 100);
        var quantityToRemove = -150;
        var reason = "Removing stock";

        // Act
        Action act = () => product.UpdateStock(quantityToRemove, reason);

        // Assert
        act.Should().Throw<DomainException>().WithMessage("Stock quantity cannot be negative.");
    }

    [Fact]
    public void Deactivate_ShouldSetStatusToInactive()
    {
        // Arrange
        var product = CreateValidProduct();

        // Act
        product.Deactivate();

        // Assert
        product.Status.Should().Be(ProductStatus.Inactive);
        product.DomainEvents.Should().ContainSingle(e => e is ProductDeactivatedEvent);
        product.DomainEvents.OfType<ProductDeactivatedEvent>().First().ProductId.Should().Be(product.Id);
    }

    [Fact]
    public void Activate_ShouldSetStatusToActive()
    {
        // Arrange
        var product = CreateValidProduct();
        product.Deactivate();

        // Act
        product.Activate();

        // Assert
        product.Status.Should().Be(ProductStatus.Active);
        product.DomainEvents.Should().ContainSingle(e => e is ProductActivatedEvent);
        product.DomainEvents.OfType<ProductActivatedEvent>().First().ProductId.Should().Be(product.Id);
    }

    [Fact]
    public void Deactivate_WhenAlreadyInactive_ShouldThrowDomainException()
    {
        // Arrange
        var product = CreateValidProduct();
        product.Deactivate();

        // Act
        Action act = () => product.Deactivate();

        // Assert
        act.Should().Throw<DomainException>().WithMessage("Product is already inactive.");
    }
    [Fact]
    public void Activate_WhenAlreadyActive_ShouldThrowDomainException()
    {
        // Arrange
        var product = CreateValidProduct();

        // Act
        Action act = () => product.Activate();

        // Assert
        act.Should().Throw<DomainException>().WithMessage("Product is already active.");
    }
    [Fact]
    public void UpdateDescription_ShouldUpdateProductDescription()
    {
        // Arrange
        var product = CreateValidProduct(description: "Old description");
        var newDescription = "New description";

        // Act
        product.UpdateDescription(newDescription);

        // Assert
        product.Description.Should().Be(newDescription);
    }

    [Fact]
    public void AddImage_WithValidImage_ShouldAddProductImage()
    {
        // Arrange
        var product = CreateValidProduct();
        var image = new ProductImage("http://example.com/image.jpg", 1);

        // Act
        product.AddImage(image);

        // Assert
        product.DomainEvents.Should().ContainSingle(e => e is ImageAddedEvent);
        product.DomainEvents.OfType<ImageAddedEvent>().First().ProductId.Should().Be(product.Id);
        product.DomainEvents.OfType<ImageAddedEvent>().First().ImageUrl.Should().Be(image.Url);
        product.DomainEvents.OfType<ImageAddedEvent>().First().Order.Should().Be(image.Order);
    }

    [Fact]
    public void AddImage_WithDuplicateOrder_ShouldThrowDomainException()
    {
        // Arrange
        var product = CreateValidProduct();
        var image1 = new ProductImage("http://example.com/image1.jpg", 1);
        var image2 = new ProductImage("http://example.com/image2.jpg", 1);
        product.AddImage(image1);

        // Act
        Action act = () => product.AddImage(image2);

        // Assert
        act.Should().Throw<DomainException>().WithMessage("An image with the same order already exists.");
    }


}
