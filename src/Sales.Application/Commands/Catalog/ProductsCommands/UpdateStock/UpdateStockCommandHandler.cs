using Sales.Application.Abstractions.Persistence;
using Sales.Domain.Common.Exceptions;

namespace Sales.Application.Commands.Catalog.ProductsCommands.UpdateStock;

public class UpdateStockCommandHandler
{
    private readonly IProductRepository _productRepository;

    public UpdateStockCommandHandler(IProductRepository productRepository)
    {
        _productRepository = productRepository;
    }

    public async Task<UpdateStockResultDto> Handle(UpdateStockCommand command, CancellationToken cancellationToken = default)
    {
        var product = await _productRepository.GetByIdAsync(command.ProductId, cancellationToken)
            ?? throw new DomainException("Product not found.");

        product.UpdateStock(command.Quantity, command.Reason);

        await _productRepository.UpdateAsync(product, cancellationToken);

        return new UpdateStockResultDto
        {
            ProductId = product.Id,
            CurrentStock = product.StockQuantity
        };
    }
}
