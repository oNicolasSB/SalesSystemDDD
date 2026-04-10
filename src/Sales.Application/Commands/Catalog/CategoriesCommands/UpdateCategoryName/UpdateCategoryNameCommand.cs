namespace Sales.Application.Commands.Catalog.CategoriesCommands.UpdateCategoryName;

public sealed class UpdateCategoryNameCommand
{
    public Guid CategoryId { get; }
    public string NewName { get; }

    public UpdateCategoryNameCommand(Guid categoryId, string newName)
    {
        CategoryId = categoryId;
        NewName = newName;
    }
}
