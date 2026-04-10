using Sales.Application.Abstractions.Persistence;
using Sales.Domain.Common.Exceptions;

namespace Sales.Application.Commands.Catalog.ProductsCommands.DeactivateProduct;

public class DeactivateProductCommandHandler
{
    private readonly IProductRepository _productRepository;

    public DeactivateProductCommandHandler(IProductRepository productRepository)
    {
        _productRepository = productRepository;
    }

    public async Task<DeactivateProductResultDto> Handle(DeactivateProductCommand command, CancellationToken cancellationToken = default)
    {
        var product = await _productRepository.GetByIdAsync(command.ProductId, cancellationToken)
            ?? throw new DomainException("Product not found.");

        product.Deactivate();

        await _productRepository.UpdateAsync(product, cancellationToken);

        return new DeactivateProductResultDto
        {
            ProductId = product.Id,
            Status = product.Status.ToString()
        };
    }
}
