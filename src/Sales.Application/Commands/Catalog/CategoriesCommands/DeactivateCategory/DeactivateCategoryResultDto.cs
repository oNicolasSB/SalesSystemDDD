namespace Sales.Application.Commands.Catalog.CategoriesCommands.ActivateCategory;

public sealed class DeactivateCategoryResultDto
{
    public Guid CategoryId { get; init; }
    public bool IsActive { get; init; }
}
