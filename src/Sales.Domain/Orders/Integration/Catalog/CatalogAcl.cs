namespace Sales.Domain.Orders.Integration.Catalog;

public sealed class CatalogAcl
{
    public (string productName, decimal unitPrice) TranslateProduct(ProductDto product)
    {
        return (product.Name, product.Price);
    }
}
