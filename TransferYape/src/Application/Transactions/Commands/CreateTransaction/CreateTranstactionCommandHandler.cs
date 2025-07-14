using MassTransit;
using MediatR;
using TransferYape.Application.Messages.Transactions;
using TransferYape.Application.Repositories;
using TransferYape.Application.Transactions.Models;
using TransferYape.Domain.Transactions.Entities;
using TransferYape.Domain.Transactions.Repositories;

namespace TransferYape.Application.Transactions.Commands.CreateTransaction;
internal sealed class CreateTranstactionCommandHandler : IRequestHandler<CreateTranstactionCommand, TransactionCreatedResponse>
{
    private readonly ITransactionRepository _transactionRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IPublishEndpoint _publishEndpoint;

    public CreateTranstactionCommandHandler(ITransactionRepository transactionRepository, IUnitOfWork unitOfWork, IPublishEndpoint publishEndpoint)
    {
        _transactionRepository = transactionRepository;
        _unitOfWork = unitOfWork;
        _publishEndpoint = publishEndpoint;
    }

    public async Task<TransactionCreatedResponse> Handle(CreateTranstactionCommand request, CancellationToken cancellationToken)
    {
        var transaction = Transaction.Create(request.SourceAccountId, request.SourceAccountId, request.Value);
        await _transactionRepository.AddAsync(transaction, cancellationToken);
        await _unitOfWork.CommitTransactionAsync();

        await _publishEndpoint.Publish(new TransactionCreated(transaction.Id, transaction.CreatedAt, transaction.SourceAccountId), cancellationToken);

        return new TransactionCreatedResponse(transaction.Id, transaction.CreatedAt);
    }
}
