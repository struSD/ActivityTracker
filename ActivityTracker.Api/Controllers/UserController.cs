using System.Threading;
using System.Threading.Tasks;

using ActivityTracker.Contract.Http;
using ActivityTracker.Domain.Commands;

using MediatR;

using Microsoft.AspNetCore.Mvc;

namespace ActivityTracker.Api.Controller;

[Route("api/user")]
public class UserController : ControllerBase
{
    private readonly IMediator _mediator;
    public UserController(IMediator mediator)
    {
        _mediator = mediator;
    }
    [HttpPut]
    public async Task<IActionResult> CreateUser([FromBody] CreateUserRequest request,
        CancellationToken cancellationToken)
    {
        var command = new CreateUserCommand
        {
            Name = request.Name
        };
        var result = await _mediator.Send(command, cancellationToken);
        var responce = new CreateUserResponce
        {
            Id = result.UserId
        };
        return Created("http://todo.com", responce);
    }

}