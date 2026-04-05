using Sales.Domain.Catalog.Entities;
using Sales.Domain.Catalog.Events;
using Sales.Domain.Common.Exceptions;

namespace Sales.Domain.Tests.Catalog.Entities;

public class CategoryTest
{
    [Fact]
    public void CreateCategory_ValidData_ShouldSucceed()
    {
        // Arrange
        var name = "Electronics";
        var description = "All electronic items";

        // Act
        var category = new Category(name, description);

        // Assert
        category.Should().NotBeNull();
        category.Name.Should().Be(name);
        category.Description.Should().Be(description);
        category.IsActive.Should().BeTrue();
        category.DomainEvents.Should().BeEmpty();
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData("  ")]
    [InlineData("ab")]
    public void CreateCategory_InvalidName_ShouldThrowDomainException(string? invalidName)
    {
        // Act
        Action act = () => new Category(invalidName!);

        // Assert
        act.Should().Throw<DomainException>()
            .WithMessage("*name*");
    }

    [Fact]
    public void UpdateName_ValidName_ShouldSucceed()
    {
        // Arrange
        var category = new Category("Books");

        // Act
        category.UpdateName("New Books");

        // Assert
        category.Name.Should().Be("New Books");
        category.UpdatedAt.Should().NotBeNull();
    }

    [Fact]
    public void UpdateName_InvalidName_ShouldThrowDomainException()
    {
        // Arrange
        var category = new Category("Books");

        // Act
        Action act = () => category.UpdateName("  ");

        // Assert
        act.Should().Throw<DomainException>()
            .WithMessage("*newName*");
    }

    [Fact]
    public void UpdateDescription_ShouldSucceed()
    {
        // Arrange
        var category = new Category("Books");

        // Act
        category.UpdateDescription("All kinds of books");

        // Assert
        category.Description.Should().Be("All kinds of books");
        category.UpdatedAt.Should().NotBeNull();
    }

    [Fact]
    public void Activate_ShouldActivateCategory()
    {
        // Arrange
        var category = new Category("Books");
        category.Deactivate();

        // Act
        category.Activate();

        // Assert
        category.IsActive.Should().BeTrue();
        category.DomainEvents.Should().ContainSingle(e => e is CategoryActivatedEvent);
    }

    [Fact]
    public void Activate_AlreadyActive_ShouldThrowDomainException()
    {
        // Arrange
        var category = new Category("Books");

        // Act
        Action act = () => category.Activate();

        // Assert
        act.Should().Throw<DomainException>()
            .WithMessage("*already active*");
    }

    [Fact]
    public void Deactivate_ShouldDeactivateCategory()
    {
        // Arrange
        var category = new Category("Books");

        // Act
        category.Deactivate();

        // Assert
        category.IsActive.Should().BeFalse();
        category.DomainEvents.Should().ContainSingle(e => e is CategoryDeactivatedEvent);
    }

    [Fact]
    public void Deactivate_AlreadyInactive_ShouldThrowDomainException()
    {
        // Arrange
        var category = new Category("Books");
        category.Deactivate();

        // Act
        Action act = () => category.Deactivate();

        // Assert
        act.Should().Throw<DomainException>()
            .WithMessage("*already inactive*");
    }

    [Fact]
    public void DomainEvents_ShouldBeAbleToCleanUp()
    {
        // Arrange
        var category = new Category("Books");

        // Act
        category.Deactivate();
        category.DomainEvents.Should().ContainSingle(e => e is CategoryDeactivatedEvent);
        
        category.ClearDomainEvents();

        // Assert
        category.DomainEvents.Should().BeEmpty();
    }
}
