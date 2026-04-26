namespace Sales.Application.Commands.Catalog.CategoriesCommands.CreateCategory;

public sealed class CreateCategoryResultDto
{
    public Guid CategoryId { get; init; }
    public string Name { get; init; } = string.Empty;
    public bool IsActive { get; init; }

}
