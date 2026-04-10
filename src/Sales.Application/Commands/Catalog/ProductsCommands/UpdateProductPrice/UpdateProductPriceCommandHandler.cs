using Sales.Application.Abstractions.Persistence;
using Sales.Domain.Catalog.ValueObjects;
using Sales.Domain.Common.Exceptions;

namespace Sales.Application.Commands.Catalog.ProductsCommands.UpdateProductPrice;

public sealed class UpdateProductPriceCommandHandler
{
    private readonly IProductRepository _productRepository;

    public UpdateProductPriceCommandHandler(IProductRepository productRepository)
    {
        _productRepository = productRepository;
    }

    public async Task<UpdateProductPriceResultDto> Handle(UpdateProductPriceCommand command, CancellationToken cancellationToken = default)
    {
        var product = await _productRepository.GetByIdAsync(command.ProductId, cancellationToken) ?? throw new DomainException("Product not found.");

        ProductPrice newPrice = new(command.NewPrice);

        product.UpdatePrice(newPrice);

        await _productRepository.UpdateAsync(product, cancellationToken);

        return new UpdateProductPriceResultDto
        {
            ProductId = product.Id,
            NewPrice = product.Price.Value
        };
    }
}
