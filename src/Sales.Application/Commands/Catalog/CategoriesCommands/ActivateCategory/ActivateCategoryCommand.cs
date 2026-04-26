namespace Sales.Application.Commands.Catalog.CategoriesCommands.ActivateCategory;

public sealed class ActivateCategoryCommand
{
    public Guid CategoryId { get; }

    public ActivateCategoryCommand(Guid categoryId)
    {
        CategoryId = categoryId;
    }
}
