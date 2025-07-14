using AntifraudYape.Application.Messages.Transactions;
using AntifraudYape.Application.Transactions.Repositories;
using AntifraudYape.Domain;
using AntifraudYape.Domain.Transactions.Enums;
using MassTransit;
using MediatR;

namespace AntifraudYape.Application.Transactions.Query.GetTransactions;
internal sealed class GetTransactionsQueryHandler : IRequestHandler<GetTransactionsQuery>
{
    private readonly ITransactionReadModelRepository _transactionRepository;
    private readonly IPublishEndpoint _publishEndpoint;

    public GetTransactionsQueryHandler(ITransactionReadModelRepository transactionRepository, IPublishEndpoint publishEndpoint)
    {
        _transactionRepository = transactionRepository;
        _publishEndpoint = publishEndpoint;
    }

    public async Task Handle(GetTransactionsQuery request, CancellationToken cancellationToken)
    {
        TransactionValidated validated = new TransactionValidated(request.Id, TransactionStatus.Approved.Value);
  
        var transactions = await _transactionRepository.GetByDateAsync(request.CreatedAt, request.SourceAccountId, cancellationToken);

        if (transactions.Single(t => t.Id == request.Id).Value > Globals.MaximumAmountPerTransaction || transactions.Sum(t => t.Value) > Globals.MaximumAccumulatedAmountPerDay)
            validated = validated with { StatusId = TransactionStatus.Rejected.Value };

        await _publishEndpoint.Publish(validated, cancellationToken);
    }
}
