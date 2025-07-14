using FluentAssertions;
using AntifraudYape.Domain.Transactions.Entities;
using AntifraudYape.Domain.Transactions.Enums;

namespace AntifraudYape.Domain.UniTests.Transactions.Entities;
public sealed class TransactionTests
{

    [Fact]
    public void Create_WhenCreateTransaction_ShouldSuccessShouldSuccess()
    {
        // Arrange & Act 
        var sourceAccountId = Guid.NewGuid();
        var targetAccountId = Guid.NewGuid();
        var value = 105.5m;
        var transaction = Transaction.Create(sourceAccountId, targetAccountId, value);

        // Assert
        transaction.Should().NotBeNull();
        transaction.Status.Should().Be(TransactionStatus.Pending);
        transaction.SourceAccountId.Should().Be(sourceAccountId);
        transaction.TargetAccountId.Should().Be(targetAccountId);
        transaction.Value.Should().Be(value);
    }

    [Fact]
    public void Create_WhenValueIsLessThanZero_ShouldThrowValidationException()
    {
        // Arrange & Act 
        var sourceAccountId = Guid.NewGuid();
        var targetAccountId = Guid.NewGuid();
        var value = 0m;
        var exception = Record.Exception(() => Transaction.Create(sourceAccountId, targetAccountId, value));

        // Assert
        exception.Should().NotBeNull();
        exception.Should().BeOfType<ArgumentException>().And
            .Match<ArgumentException>(e =>
                e.Message == "Value must be greater than zero.");
    }

    [Fact]
    public void Update_WhenUpdateTransactionStatus_ShouldSuccess()
    {
        // Act
        var sourceAccountId = Guid.NewGuid();
        var targetAccountId = Guid.NewGuid();
        var value = 105.5m;
        var transaction = Transaction.Create(sourceAccountId, targetAccountId, value);

        transaction.UpdateStatus(TransactionStatus.Approved.Value);

        // Assert
        transaction.Should().NotBeNull();
        transaction.Status.Should().Be(TransactionStatus.Approved);
    }
}
