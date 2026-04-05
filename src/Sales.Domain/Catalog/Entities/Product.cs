using Sales.Domain.Catalog.Enums;
using Sales.Domain.Catalog.Events;
using Sales.Domain.Catalog.ValueObjects;
using Sales.Domain.Common.Base;
using Sales.Domain.Common.Exceptions;
using Sales.Domain.Common.Validation;

namespace Sales.Domain.Catalog.Entities;

public sealed class Product : AggregateRoot
{
    public ProductName Name { get; private set; }
    public ProductCode Code { get; private set; }
    public ProductPrice Price { get; private set; }
    public string? Description { get; private set; }
    public Guid? CategoryId { get; private set; }
    public ProductStatus Status { get; private set; }
    public int StockQuantity { get; private set; }
    private readonly List<ProductImage> _images = [];
    public IReadOnlyCollection<ProductImage> Images => _images.AsReadOnly();

    public Product(ProductName name, ProductCode code, ProductPrice price, Guid categoryId, int stockQuantity = 0, string? description = null)
    {
        Guard.AgainstNull(name, nameof(ProductName));
        Guard.AgainstNull(code, nameof(ProductCode));
        Guard.AgainstNull(price, nameof(ProductPrice));
        Guard.AgainstEmptyGuid(categoryId, nameof(CategoryId));
        Guard.Against<DomainException>(stockQuantity < 0, "Stock quantity cannot be negative.");
        Name = name;
        Code = code;
        Price = price;
        Description = description;
        CategoryId = categoryId;
        StockQuantity = stockQuantity;

        Status = ProductStatus.Active;
    }

    public void UpdateName(ProductName name)
    {
        Guard.AgainstNull(name, nameof(ProductName));
        Name = name;
        UpdateDate();
    }

    public void UpdatePrice(ProductPrice price)
    {
        Guard.AgainstNull(price, nameof(ProductPrice));
        decimal oldPrice = Price.Value;
        decimal newPrice = price.Value;

        Price = price;
        UpdateDate();

        AddDomainEvent(new ProductPriceChangedEvent(Id, oldPrice, newPrice));
    }

    public void UpdateCategory(Guid categoryId)
    {
        Guard.AgainstEmptyGuid(categoryId, nameof(CategoryId));
        CategoryId = categoryId;
        UpdateDate();
    }

    public void UpdateDescription(string? description)
    {
        Description = description?.Trim();
        UpdateDate();
    }

    public void UpdateStock(int quantity, string reason)
    {
        Guard.AgainstNullOrWhitespace(reason, nameof(reason));
        Guard.Against<DomainException>(StockQuantity + quantity < 0, "Stock quantity cannot be negative.");

        StockQuantity += quantity;
        UpdateDate();

        AddDomainEvent(new StockUpdatedEvent(Id, StockQuantity, reason));
    }

    public void Activate()
    {
        Guard.Against<DomainException>(Status == ProductStatus.Active, "Product is already active.");

        Status = ProductStatus.Active;

        UpdateDate();
        AddDomainEvent(new ProductActivatedEvent(Id));
    }

    public void Deactivate()
    {
        Guard.Against<DomainException>(Status == ProductStatus.Inactive, "Product is already inactive.");

        Status = ProductStatus.Inactive;

        UpdateDate();
        AddDomainEvent(new ProductDeactivatedEvent(Id));
    }

    public void AddImage(ProductImage image)
    {
        Guard.AgainstNull(image, nameof(ProductImage));
        Guard.Against<DomainException>(_images.Any(i => i.Order == image.Order), "An image with the same order already exists.");
        _images.Add(image);
        UpdateDate();
        AddDomainEvent(new ImageAddedEvent(Id, image.Url, image.Order));
    }

}
