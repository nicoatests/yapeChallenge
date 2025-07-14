using Microsoft.Extensions.Logging;
using NSubstitute;
using TransferYape.Application.Repositories;
using TransferYape.Application.Transactions.Commands.UpdateTransactionStatus;
using TransferYape.Domain.Transactions.Entities;
using TransferYape.Domain.Transactions.Enums;
using TransferYape.Domain.Transactions.Repositories;

namespace TransferYape.Application.UnitTests.Transactions.Commands.UpdateTransactionStatus;
public sealed class UpdateTransactionStatusCommandHandlerTests
{
    private readonly ITransactionRepository _transactionRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly UpdateTransactionStatusCommandHandler _commandHandler;
    private readonly ILogger<UpdateTransactionStatusCommandHandler> _logger;

    public UpdateTransactionStatusCommandHandlerTests()
    {
        _transactionRepository = Substitute.For<ITransactionRepository>();
        _unitOfWork = Substitute.For<IUnitOfWork>();
        _logger = Substitute.For<ILogger<UpdateTransactionStatusCommandHandler>>();

        _commandHandler = new UpdateTransactionStatusCommandHandler(_transactionRepository, _unitOfWork, _logger);
    }

    [Fact]
    public async Task Handle_WhenUpdateTranstactionStatus_ShouldSuccess()
    {
        //Arrange
        var transaction = Transaction.Create(Guid.NewGuid(), Guid.NewGuid(), 200.5m);
        var command = new UpdateTransactionStatusCommand(transaction.Id, TransactionStatus.Approved.Value);
        _transactionRepository.GetByIdAsync(command.Id, Arg.Any<CancellationToken>())
           .Returns(transaction);

        //Act
        await _commandHandler.Handle(command, default);

        //Assert 
        await _unitOfWork.Received(1).CommitTransactionAsync();
    }

    [Fact]
    public async Task Handle_WhenTransactionDoesntExists_ShouldShowWarningLog()
    {
        //Arrange
        var transaction = Transaction.Create(Guid.NewGuid(), Guid.NewGuid(), 200.5m);
        var command = new UpdateTransactionStatusCommand(transaction.Id, TransactionStatus.Approved.Value);

        //Act
        await _commandHandler.Handle(command, default);

        //Assert 
        _logger.Received(1).Log(
               LogLevel.Warning,
               Arg.Any<EventId>(),
               Arg.Is<object>(o => o.ToString().Contains($"The transaction with id: {transaction.Id} does not exists")),
               null,
               Arg.Any<Func<object, Exception, string>>());
    }
}