using Sales.Application.Abstractions.Persistence;
using Sales.Domain.Common.Exceptions;
using Sales.Domain.Common.Validation;

namespace Sales.Application.Commands.Catalog.ProductsCommands.UpdateProductCategory;

public sealed class UpdateProductCategoryCommandHandler
{
    private readonly IProductRepository _productRepository;
    private readonly ICategoryRepository _categoryRepository;

    public UpdateProductCategoryCommandHandler(IProductRepository productRepository, ICategoryRepository categoryRepository)
    {
        _productRepository = productRepository;
        _categoryRepository = categoryRepository;
    }

    public async Task<UpdateProductCategoryResultDto> HandleAsync(UpdateProductCategoryCommand command, CancellationToken cancellationToken = default)
    {
        var category = await _categoryRepository.GetByIdAsync(command.NewCategoryId, cancellationToken) ?? throw new DomainException("Category not found.");
        Guard.Against<DomainException>(!category.IsActive, "Cannot assign an inactive category to a product.");

        var product = await _productRepository.GetByIdAsync(command.ProductId, cancellationToken) ?? throw new DomainException("Product not found.");

        product.UpdateCategory(command.NewCategoryId);

        await _productRepository.UpdateAsync(product, cancellationToken);

        return new UpdateProductCategoryResultDto()
        {
            ProductId = product.Id,
            CategoryId = product.CategoryId!.Value
        };
    }
}
