using Sales.Application.Abstractions.Persistence;
using Sales.Domain.Catalog.Entities;

namespace Sales.Application.Commands.Catalog.CategoriesCommands.CreateCategory;

public sealed class CreateCategoryCommandHandler
{
    private readonly ICategoryRepository _categoryRepository;

    public CreateCategoryCommandHandler(ICategoryRepository categoryRepository)
    {
        _categoryRepository = categoryRepository;
    }

    public async Task<CreateCategoryResultDto> Handle(CreateCategoryCommand command, CancellationToken cancellationToken)
    {
        var category = new Category(command.Name, command.Description);

        await _categoryRepository.AddAsync(category, cancellationToken);

        return new CreateCategoryResultDto
        {
            CategoryId = category.Id,
            Name = category.Name,
            IsActive = category.IsActive
        };
    }
}
