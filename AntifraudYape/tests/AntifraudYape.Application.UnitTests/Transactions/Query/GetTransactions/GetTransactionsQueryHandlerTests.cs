using AntifraudYape.Application.Messages.Transactions;
using AntifraudYape.Application.Transactions.Query.GetTransactions;
using AntifraudYape.Application.Transactions.Repositories;
using AntifraudYape.Domain.Transactions.Entities;
using AntifraudYape.Domain.Transactions.Enums;
using MassTransit;
using NSubstitute;

namespace AntifraudYape.Application.UnitTests.Transactions.Query.GetTransactions;
public sealed class GetTransactionsQueryHandlerTests
{
    private readonly ITransactionReadModelRepository _transactionRepository;
    private readonly IPublishEndpoint _publishEndpoint;
    private readonly GetTransactionsQueryHandler _commandHandler;

    public GetTransactionsQueryHandlerTests()
    {
        _transactionRepository = Substitute.For<ITransactionReadModelRepository>();
        _publishEndpoint = Substitute.For<IPublishEndpoint>();

        _commandHandler = new GetTransactionsQueryHandler(_transactionRepository, _publishEndpoint);
    }

    [Fact]
    public async Task Handle_WhenValidateTranstaction_ShouldReturnStatusApproved()
    {
        //Arrange
        var sourceAccountId = Guid.NewGuid();
        var targetAccountId = Guid.NewGuid();
        var transaction1 = Transaction.Create(sourceAccountId, targetAccountId, 100.5m);
        var transaction2 = Transaction.Create(sourceAccountId, targetAccountId, 100m);
        var transaction3 = Transaction.Create(sourceAccountId, targetAccountId, 220m);
        var command = new GetTransactionsQuery(transaction1.Id, transaction1.CreatedAt, transaction1.SourceAccountId);
        _transactionRepository.GetByDateAsync(transaction1.CreatedAt, sourceAccountId, Arg.Any<CancellationToken>())
            .Returns(new List<Transaction> { transaction1, transaction2, transaction3 });

        //Act
        await _commandHandler.Handle(command, default);

        //Assert 
        await _publishEndpoint.Received(1)
            .Publish(Arg.Is<TransactionValidated>(
                x => x.StatusId == TransactionStatus.Approved.Value),
                Arg.Any<CancellationToken>());
    }

    [Fact]
    public async Task Handle_WhenValidateTranstactionPerDay_ShouldReturnStatusRejected()
    {
        //Arrange
        var sourceAccountId = Guid.NewGuid();
        var targetAccountId = Guid.NewGuid();
        var transaction1 = Transaction.Create(sourceAccountId, targetAccountId, 2000m);
        var transaction2 = Transaction.Create(sourceAccountId, targetAccountId, 2000m);
        var transaction3 = Transaction.Create(sourceAccountId, targetAccountId, 2000m);
        var transaction4 = Transaction.Create(sourceAccountId, targetAccountId, 2000m);
        var transaction5 = Transaction.Create(sourceAccountId, targetAccountId, 2000m);
        var transaction6 = Transaction.Create(sourceAccountId, targetAccountId, 2000m);
        var transaction7 = Transaction.Create(sourceAccountId, targetAccountId, 2000m);
        var transaction8 = Transaction.Create(sourceAccountId, targetAccountId, 2000m);
        var transaction9 = Transaction.Create(sourceAccountId, targetAccountId, 2000m);
        var transaction10 = Transaction.Create(sourceAccountId, targetAccountId, 2000m);
        var transaction11 = Transaction.Create(sourceAccountId, targetAccountId, 2000m);

        var command = new GetTransactionsQuery(transaction1.Id, transaction1.CreatedAt, transaction1.SourceAccountId);
        _transactionRepository.GetByDateAsync(transaction1.CreatedAt, sourceAccountId, Arg.Any<CancellationToken>())
            .Returns(new List<Transaction> { transaction1, transaction2, transaction3, transaction4, transaction5, transaction6,
            transaction7, transaction8, transaction9, transaction10, transaction11});

        //Act
        await _commandHandler.Handle(command, default);

        //Assert 
        await _publishEndpoint.Received(1)
            .Publish(Arg.Is<TransactionValidated>(
                x => x.StatusId == TransactionStatus.Rejected.Value),
                Arg.Any<CancellationToken>());
    }

    [Fact]
    public async Task Handle_WhenValidateTranstactionLimit_ShouldReturnStatusRejected()
    {
        //Arrange
        var sourceAccountId = Guid.NewGuid();
        var targetAccountId = Guid.NewGuid();
        var transaction1 = Transaction.Create(sourceAccountId, targetAccountId, 4000m);

        var command = new GetTransactionsQuery(transaction1.Id, transaction1.CreatedAt, transaction1.SourceAccountId);
        _transactionRepository.GetByDateAsync(transaction1.CreatedAt, sourceAccountId, Arg.Any<CancellationToken>())
            .Returns(new List<Transaction> { transaction1 });

        //Act
        await _commandHandler.Handle(command, default);

        //Assert 
        await _publishEndpoint.Received(1)
            .Publish(Arg.Is<TransactionValidated>(
                x => x.StatusId == TransactionStatus.Rejected.Value),
                Arg.Any<CancellationToken>());
    }
}
