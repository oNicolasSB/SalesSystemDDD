using Sales.Application.Abstractions.Persistence;
using Sales.Domain.Catalog.ValueObjects;
using Sales.Domain.Common.Exceptions;

namespace Sales.Application.Commands.Catalog.ProductsCommands.UpdateProductName;

public sealed class UpdateProductNameCommandHandler
{
    private readonly IProductRepository _productRepository;

    public UpdateProductNameCommandHandler(IProductRepository productRepository)
    {
        _productRepository = productRepository;
    }

    public async Task<UpdateProductNameResultDto> Handle(UpdateProductNameCommand command, CancellationToken cancellationToken = default)
    {
        var product = await _productRepository.GetByIdAsync(command.ProductId, cancellationToken)
            ?? throw new DomainException("Product not found.");

        ProductName newName = new(command.NewName);

        product.UpdateName(newName);

        await _productRepository.UpdateAsync(product, cancellationToken);

        return new UpdateProductNameResultDto
        {
            ProductId = product.Id,
            Name = product.Name.Value
        };
    }
}

