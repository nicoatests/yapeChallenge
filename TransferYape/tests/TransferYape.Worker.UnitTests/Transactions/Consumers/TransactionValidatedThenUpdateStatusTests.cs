using AutoFixture;
using MassTransit;
using MediatR;
using Microsoft.Extensions.Logging;
using NSubstitute;
using TransferYape.Application.Messages.Transactions;
using TransferYape.Application.Transactions.Commands.UpdateTransactionStatus;
using TransferYape.Worker.Transactions.Consumers;

namespace TransferYape.Worker.UnitTests.Transactions.Consumers;

public class TransactionValidatedThenUpdateStatusTests
{
    private readonly IFixture _fixture;
    private readonly ISender _mediatorMock;
    private readonly ILogger<TransactionValidatedThenUpdateStatus> _loggerMock;
    private readonly TransactionValidatedThenUpdateStatus _consumer;

    public TransactionValidatedThenUpdateStatusTests()
    {
        _fixture = new Fixture();
        _mediatorMock = Substitute.For<ISender>();
        _loggerMock = Substitute.For<ILogger<TransactionValidatedThenUpdateStatus>>();
        _consumer = new TransactionValidatedThenUpdateStatus(_mediatorMock, _loggerMock);
    }

    [Fact]
    public async Task Consumer_WhenTransactionIsValidated_ShouldUpdateTransaction()
    {
        // Arrange
        var transactionValidated = _fixture.Create<TransactionValidated>();
        var context = Substitute.For<ConsumeContext<TransactionValidated>>();
        context.Message.Returns(transactionValidated);

        // Act
        await _consumer.Consume(context);

        // Assert
        await _mediatorMock.Received(1).Send(Arg.Any<UpdateTransactionStatusCommand>(), CancellationToken.None);
    }
}

