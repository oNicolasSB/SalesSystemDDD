namespace Sales.Application.Commands.Catalog.CategoriesCommands.UpdateCategoryName;

public sealed class UpdateCategoryNameResultDto
{
    public Guid CategoryId { get; init; }
    public string CurrentName { get; init; } = string.Empty;
}
