using FluentValidation.TestHelper;
using TransferYape.Application.Transactions.Commands.CreateTransaction;

namespace TransferYape.Application.UnitTests.Transactions.Commands.CreateTransaction;
public class CreateTranstactionCommandHandlerValidatorTests
{
    private readonly CreateTranstactionCommandHandlerValidator _validator;
    public CreateTranstactionCommandHandlerValidatorTests()
    {
        _validator = new CreateTranstactionCommandHandlerValidator();
    }

    [Fact]
    public async Task Transaction_WhenAllValueAreCorrect_ShouldSucceed()
    {
        // Arrange
        var command = new CreateTranstactionCommand(
            Guid.NewGuid(),
            Guid.NewGuid(),
            100.00m
        );

        // Act
        var result = await _validator.TestValidateAsync(command, opt => opt.IncludeProperties(p => p.Value));

        // Assert
        result.ShouldNotHaveAnyValidationErrors();
    }

   
    [Fact]
    public async Task Transaction_WhenSourceAccountIdIsEmpty_ShouldReturnError()
    {
        // Arrange
        var command = new CreateTranstactionCommand(
            Guid.Empty,
            Guid.NewGuid(),
            100.00m
        );

        // Act
        var result = await _validator.TestValidateAsync(command, opt => opt.IncludeProperties(p => p.SourceAccountId));

        // Assert
        result.ShouldHaveValidationErrorFor(p => p.SourceAccountId)
            .WithErrorMessage("Source Accound Id must not be empty.");
    }

    [Fact]
    public async Task Transaction_WhenTransactionTargetAccountIdIsEmpty_ShouldReturnError()
    {
        // Arrange
        var command = new CreateTranstactionCommand(
            Guid.NewGuid(),
            Guid.Empty,
            100.00m
        );

        // Act
        var result = await _validator.TestValidateAsync(command, opt => opt.IncludeProperties(p => p.TargetAccountId));

        // Assert
        result.ShouldHaveValidationErrorFor(p => p.TargetAccountId)
            .WithErrorMessage("Target Account Id must not be empty.");
    }

    [Fact]
    public async Task Transaction_WhenTransactionTargetAccountIdEmptyGuid_ShouldReturnError()
    {
        // Arrange
        var command = new CreateTranstactionCommand(
            Guid.NewGuid(),
            Guid.NewGuid(),
            0
        );

        // Act
        var result = await _validator.TestValidateAsync(command, opt => opt.IncludeProperties(p => p.Value));

        // Assert
        result.ShouldHaveValidationErrorFor(p => p.Value)
            .WithErrorMessage("Value must be greater than zero.");
    }
}
