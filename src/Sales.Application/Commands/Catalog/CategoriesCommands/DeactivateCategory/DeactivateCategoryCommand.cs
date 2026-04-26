namespace Sales.Application.Commands.Catalog.CategoriesCommands.ActivateCategory;

public sealed class DeactivateCategoryCommand
{
    public Guid CategoryId { get; }

    public DeactivateCategoryCommand(Guid categoryId)
    {
        CategoryId = categoryId;
    }
}
