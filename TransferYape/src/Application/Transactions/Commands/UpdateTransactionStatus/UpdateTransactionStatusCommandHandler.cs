using MediatR;
using Microsoft.Extensions.Logging;
using TransferYape.Application.Repositories;
using TransferYape.Domain.Transactions.Enums;
using TransferYape.Domain.Transactions.Repositories;

namespace TransferYape.Application.Transactions.Commands.UpdateTransactionStatus;
public sealed class UpdateTransactionStatusCommandHandler : IRequestHandler<UpdateTransactionStatusCommand>
{
    private readonly ITransactionRepository _transactionRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly ILogger<UpdateTransactionStatusCommandHandler> _logger;

    public UpdateTransactionStatusCommandHandler(ITransactionRepository transactionRepository, IUnitOfWork unitOfWork, ILogger<UpdateTransactionStatusCommandHandler> logger)
    {
        _transactionRepository = transactionRepository;
        _unitOfWork = unitOfWork;
        _logger = logger;
    }

    public async Task Handle(UpdateTransactionStatusCommand request, CancellationToken cancellationToken)
    {
        var transaction = await _transactionRepository.GetByIdAsync(request.Id, cancellationToken);
        if (transaction is null)
        {
            _logger.LogWarning("The transaction with id: {request.Id} does not exists", request.Id);
            return;
        }
        transaction.UpdateStatus(request.StatusId);
        await _unitOfWork.CommitTransactionAsync();
        _logger.LogInformation("The transaction with id: {request.Id} was successfull updated to {status}", request.Id, TransactionStatus.List.Single(t => t.Value == request.StatusId).Name);
    }
}
