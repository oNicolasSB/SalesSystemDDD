using Sales.Application.Abstractions.Persistence;
using Sales.Domain.Catalog.Entities;
using Sales.Domain.Catalog.ValueObjects;
using Sales.Domain.Common.Exceptions;
using Sales.Domain.Common.Validation;

namespace Sales.Application.Commands.Catalog.ProductsCommands.CreateProduct;

public class CreateProductCommandHandler
{
    private readonly IProductRepository _productRepository;
    private readonly ICategoryRepository _categoryRepository;

    public CreateProductCommandHandler(IProductRepository productRepository, ICategoryRepository categoryRepository)
    {
        _productRepository = productRepository;
        _categoryRepository = categoryRepository;
    }

    public async Task<CreateProductResultDto> HandleAsync(CreateProductCommand command, CancellationToken cancellationToken = default)
    {
        var category = await _categoryRepository.GetByIdAsync(command.CategoryId, cancellationToken) ?? throw new DomainException("Category not found.");
        Guard.Against<DomainException>(!category.IsActive, "Cannot create a product with an inactive category.");

        ProductName productName = new(command.Name);
        ProductCode productCode = new(command.Code);
        ProductPrice productPrice = new(command.Price);

        Product product = new Product(productName, productCode, productPrice, command.CategoryId, command.InitialStock, command.Description);

        await _productRepository.AddAsync(product, cancellationToken);

        return new CreateProductResultDto()
        {
            ProductId = product.Id,
            Name = product.Name.Value,
            Price = product.Price.Value,
            Status = product.Status.ToString()
        };
    }
}
