using Microsoft.AspNetCore.Mvc;
using TransferYape.Application.Transactions.Commands.CreateTransaction;
using TransferYape.Application.Transactions.Models;
using TransferYape.Application.Utils;

namespace TransferYape.Api.Controllers;


public class TransactionsController : ApiControllerBase
{
    [HttpPost]
    [ProducesResponseType(typeof(TransactionCreatedResponse), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(CustomValidationDetails), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult> CreateTranstaction([FromBody] NewTransaction newTrasnfer,
        CancellationToken cancellationToken)
    {
        var response = await Mediator.Send(new CreateTranstactionCommand(newTrasnfer.SourceAccountId, newTrasnfer.TargetAccountId, newTrasnfer.Value), cancellationToken);
        return Created(string.Empty, response);
    }
}
