using Sales.Application.Abstractions.Persistence;
using Sales.Domain.Common.Exceptions;

namespace Sales.Application.Commands.Catalog.CategoriesCommands.ActivateCategory;

public class DeactivateCategoryCommandHandler
{
    private readonly ICategoryRepository _categoryRepository;

    public DeactivateCategoryCommandHandler(ICategoryRepository categoryRepository)
    {
        _categoryRepository = categoryRepository;
    }

    public async Task<ActivateCategoryResultDto> Handle(DeactivateCategoryCommand command, CancellationToken cancellationToken)
    {
        var category = await _categoryRepository.GetByIdAsync(command.CategoryId, cancellationToken)
            ?? throw new DomainException("Category not found.");

        category.Activate();

        await _categoryRepository.UpdateAsync(category, cancellationToken);

        return new ActivateCategoryResultDto
        {
            CategoryId = category.Id,
            IsActive = category.IsActive
        };
    }
}
