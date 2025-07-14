using AntifraudYape.Application.Messages.Transactions;
using MassTransit;
using MediatR;
using TransferYape.Application.Transactions.Commands.UpdateTransactionStatus;

namespace TransferYape.Worker.Transactions.Consumers;
public sealed class TransactionValidatedThenUpdateStatus : IConsumer<TransactionValidated>
{
    private readonly ISender _mediator;
    private readonly ILogger<TransactionValidatedThenUpdateStatus> _logger;

    public TransactionValidatedThenUpdateStatus(ISender mediator, ILogger<TransactionValidatedThenUpdateStatus> logger)
    {
        _mediator = mediator;
        _logger = logger;
    }

    public async Task Consume(ConsumeContext<TransactionValidated> context)
    {
        _logger.LogInformation("Updating status for transaction id: {context.Message.TransactionExternalId}", context.Message.TransactionExternalId);
        var command = new UpdateTransactionStatusCommand(context.Message.TransactionExternalId, context.Message.StatusId);
        await _mediator.Send(command, context.CancellationToken);
    }
}
