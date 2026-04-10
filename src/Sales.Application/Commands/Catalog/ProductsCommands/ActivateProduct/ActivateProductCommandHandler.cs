using Sales.Application.Abstractions.Persistence;
using Sales.Domain.Common.Exceptions;

namespace Sales.Application.Commands.Catalog.ProductsCommands.ActivateProduct;

public class ActivateProductCommandHandler
{
    private readonly IProductRepository _productRepository;

    public ActivateProductCommandHandler(IProductRepository productRepository)
    {
        _productRepository = productRepository;
    }

    public async Task<ActivateProductResultDto> Handle(ActivateProductCommand command, CancellationToken cancellationToken = default)
    {
        var product = await _productRepository.GetByIdAsync(command.ProductId, cancellationToken)
            ?? throw new DomainException("Product not found.");

        product.Activate();

        await _productRepository.UpdateAsync(product, cancellationToken);

        return new ActivateProductResultDto
        {
            ProductId = product.Id,
            Status = product.Status.ToString()
        };
    }
}
