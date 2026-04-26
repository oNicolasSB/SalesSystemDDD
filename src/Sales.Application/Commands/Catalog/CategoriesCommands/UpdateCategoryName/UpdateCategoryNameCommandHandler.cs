using Sales.Application.Abstractions.Persistence;
using Sales.Domain.Common.Exceptions;

namespace Sales.Application.Commands.Catalog.CategoriesCommands.UpdateCategoryName;

public sealed class UpdateCategoryNameCommandHandler
{
    private readonly ICategoryRepository _categoryRepository;

    public UpdateCategoryNameCommandHandler(ICategoryRepository categoryRepository)
    {
        _categoryRepository = categoryRepository;
    }

    public async Task<UpdateCategoryNameResultDto> Handle(UpdateCategoryNameCommand command, CancellationToken cancellationToken)
    {
        var category = await _categoryRepository.GetByIdAsync(command.CategoryId, cancellationToken)
            ?? throw new DomainException("Category not found.");

        category.UpdateName(command.NewName);

        await _categoryRepository.UpdateAsync(category, cancellationToken);

        return new UpdateCategoryNameResultDto
        {
            CategoryId = category.Id,
            CurrentName = category.Name
        };
    }
}
