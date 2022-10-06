using System;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

using ActivityTracker.Contract.Http;
using ActivityTracker.Domain.Commands;
using ActivityTracker.Domain.Queries;

using MediatR;

using Microsoft.AspNetCore.Mvc;

using Npgsql;

namespace ActivityTracker.Api.Controller;

[Route("api/user")]
public class UserController : BaseController
{
    private readonly IMediator _mediator;
    public UserController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet("{userId}")]
    public Task<IActionResult> GetUser([FromRoute] int userId,
            CancellationToken cancellationToken) =>
            SafeExecute(async () =>
        {
            var query = new UserQuery
            {
                UserId = userId
            };
            var result = await _mediator.Send(query, cancellationToken);
            var responce = new UserResponce
            {
                User = new Contract.Http.User
                {
                    Id = result.User.Id,
                    Name = result.User.Name,
                    ActivityUser = result.User.ActivityUsers.Select(a => new ActivityUser
                    {
                        ActivityId = a.ActivityId,
                        ActivityType = a.ActivityType,
                        ActivityDateTime = a.ActivityDateTime,
                        ActivityDuration = a.ActivityDuration
                    }).ToList()
                }
            };
            return Ok(responce);
        }, cancellationToken);

    [HttpPut]
    public Task<IActionResult> CreateUser([FromBody] CreateUserRequest request,
            CancellationToken cancellationToken)
    {
        return SafeExecute(async () =>
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
        }, cancellationToken);
    }
}