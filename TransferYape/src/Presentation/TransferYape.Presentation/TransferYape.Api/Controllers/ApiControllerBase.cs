using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace TransferYape.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public abstract class ApiControllerBase : ControllerBase
{
    private ISender? _mediator;

    protected ISender Mediator => _mediator ?? (_mediator = base.HttpContext.RequestServices.GetRequiredService<ISender>());
}
