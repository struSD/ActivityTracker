using System.Linq;
using System.Threading;
using System.Threading.Tasks;

using ActivityTracker.Contracts.Http;
using ActivityTracker.Domain.Commands;
using ActivityTracker.Domain.Queries;

using MediatR;

using Microsoft.AspNetCore.Mvc;

namespace ActivityTracker.Api.Controllers
{
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
                CancellationToken cancellationToken)
        {
            return SafeExecute(async () =>
            {
                UserQuery query = new()
                {
                    UserId = userId
                };
                UserQueryResult result = await _mediator.Send(query, cancellationToken);
                UserResponce responce = new()
                {
                    User = new User
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
            });
        }

        [HttpPut]
        public Task<IActionResult> CreateUser([FromBody] CreateUserRequest request,
                CancellationToken cancellationToken)
        {
            return SafeExecute(async () =>
            {
                if (!ModelState.IsValid)
                {
                    return ToActionResult(new ErrorResponse
                    {
                        Code = ErrorCode.BadRequest,
                        Message = "invalid request"
                    });
                }

                CreateUserCommand command = new()
                {
                    Name = request.Name
                };
                CreateUserCommandResult result = await _mediator.Send(command, cancellationToken);
                CreateUserResponce responce = new()
                {
                    Id = result.UserId
                };
                return Created("http://todo.com", responce);
            });
        }
    }
}