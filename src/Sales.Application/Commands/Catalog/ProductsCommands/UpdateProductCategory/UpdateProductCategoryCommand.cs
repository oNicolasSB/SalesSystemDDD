namespace Sales.Application.Commands.Catalog.ProductsCommands.UpdateProductCategory;

public sealed class UpdateProductCategoryCommand
{
    public Guid ProductId { get; }
    public Guid NewCategoryId { get; }
    public UpdateProductCategoryCommand(Guid productId, Guid newCategoryId)
    {
        ProductId = productId;
        NewCategoryId = newCategoryId;
    }

}
