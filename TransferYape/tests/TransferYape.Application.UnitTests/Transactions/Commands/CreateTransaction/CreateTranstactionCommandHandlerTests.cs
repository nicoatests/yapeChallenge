using MassTransit;
using NSubstitute;
using TransferYape.Application.Messages.Transactions;
using TransferYape.Application.Repositories;
using TransferYape.Application.Transactions.Commands.CreateTransaction;
using TransferYape.Domain.Transactions.Entities;
using TransferYape.Domain.Transactions.Repositories;

namespace TransferYape.Application.UnitTests.Transactions.Commands.CreateTransaction;
public sealed class CreateTranstactionCommandHandlerTests
{
    private readonly ITransactionRepository _transactionRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IPublishEndpoint _publishEndpoint;
    private readonly CreateTranstactionCommandHandler _commandHandler;

    public CreateTranstactionCommandHandlerTests()
    {
        _transactionRepository = Substitute.For<ITransactionRepository>();
        _unitOfWork = Substitute.For<IUnitOfWork>();
        _publishEndpoint = Substitute.For<IPublishEndpoint>();

        _commandHandler = new CreateTranstactionCommandHandler(_transactionRepository, _unitOfWork, _publishEndpoint);
    }

    [Fact]
    public async Task Handle_WhenCreateTranstaction_ShouldSuccess()
    {
        //Arrange
        var command = new CreateTranstactionCommand(Guid.NewGuid(), Guid.NewGuid(), 200);

        //Act
        var result = await _commandHandler.Handle(command, default);

        //Assert 
        await _transactionRepository
                .Received(1)
                .AddAsync(Arg.Is<Transaction>(t => t.Id == result.TransactionExternalId), Arg.Any<CancellationToken>());
        await _unitOfWork.Received(1).CommitTransactionAsync();
        await _publishEndpoint.Received(1)
            .Publish(Arg.Is<TransactionCreated>(
                x => x.Id == result.TransactionExternalId && x.CreatedAt == result.CreatedAt),
                Arg.Any<CancellationToken>());
    }
}
