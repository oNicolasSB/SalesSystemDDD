namespace Sales.Application.Commands.Catalog.CategoriesCommands.CreateCategory;

public sealed class CreateCategoryCommand
{
    public CreateCategoryCommand(string name, string description)
    {
        Name = name;
        Description = description;
    }

    public string Name { get; }
    public string Description { get; }
}
