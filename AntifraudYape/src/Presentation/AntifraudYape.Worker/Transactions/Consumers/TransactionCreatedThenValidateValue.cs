using AntifraudYape.Application.Messages.Transactions;
using AntifraudYape.Application.Transactions.Query.GetTransactions;
using MassTransit;
using MediatR;

namespace AntifraudYape.Worker.Transactions.Consumers;
public sealed class TransactionCreatedThenValidateValue : IConsumer<TransactionCreated>
{
    private readonly ISender _mediator;
    private readonly ILogger<TransactionCreatedThenValidateValue> _logger;

    public TransactionCreatedThenValidateValue(ISender mediator, ILogger<TransactionCreatedThenValidateValue> logger)
    {
        _mediator = mediator;
        _logger = logger;
    }

    public async Task Consume(ConsumeContext<TransactionCreated> context)
    {
        _logger.LogInformation("Validating status for transaction Id: {context.Message.Id}", context.Message.Id);
        var command = new GetTransactionsQuery(context.Message.Id, context.Message.CreatedAt, context.Message.SourceAccountId);
        await _mediator.Send(command, context.CancellationToken);
        _logger.LogInformation("Validated status for transaction Id: {context.Message.Id}", context.Message.Id);
    }
}
