using AntifraudYape.Application.Messages.Transactions;
using AntifraudYape.Application.Transactions.Query.GetTransactions;
using AntifraudYape.Worker.Transactions.Consumers;
using MassTransit;
using MediatR;
using Microsoft.Extensions.Logging;
using NSubstitute;

namespace AntifraudYape.Worker.Tests.Transactions.Consumers;

public class TransactionCreatedThenValidateValueTests
{
    private readonly ISender _mediatorMock;
    private readonly ILogger<TransactionCreatedThenValidateValue> _loggerMock;
    private readonly TransactionCreatedThenValidateValue _consumer;

    public TransactionCreatedThenValidateValueTests()
    {
        _mediatorMock = Substitute.For<ISender>();
        _loggerMock = Substitute.For<ILogger<TransactionCreatedThenValidateValue>>();
        _consumer = new TransactionCreatedThenValidateValue(_mediatorMock, _loggerMock);
    }

    [Fact]
    public async Task Consumer_WhenNeedToValidateTransaction_ShouldValidate()
    {
        // Arrange
        var transactionValidated = new TransactionCreated(Guid.NewGuid(), DateOnly.FromDateTime(DateTime.Now), Guid.NewGuid());
        var context = Substitute.For<ConsumeContext<TransactionCreated>>();
        context.Message.Returns(transactionValidated);

        // Act
        await _consumer.Consume(context);

        // Assert
        await _mediatorMock.Received(1).Send(Arg.Any<GetTransactionsQuery>(), CancellationToken.None);
    }
}