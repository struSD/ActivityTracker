using System.ComponentModel.DataAnnotations;
using System.Threading;
using System.Threading.Tasks;

using ActivityTracker.Contract.Http;
using ActivityTracker.Domain.Commands;
using ActivityTracker.Domain.Queries;

using MediatR;

using Microsoft.AspNetCore.Mvc;

namespace ActivityTracker.Api.Controller;

[Route("api/activity")]
public class ActivityController : BaseController
{
    private readonly IMediator _mediator;

    public ActivityController(IMediator mediator)
    {
        _mediator = mediator;
    }
    [HttpGet("allActivity")]
    public async Task<IActionResult> GetAllAllActivity()
    {
        var result = await _mediator.Send(new AllActivityQuery());
        return Ok(result);
    }

    [HttpGet("activityByName")]
    public async Task<IActionResult> GetById(string name)
    {
        var product = await _mediator.Send(new GetProductByIdQuery(name));
        return Ok(product);
    }

    [HttpPut]
    public Task<IActionResult> CreateActivity([FromBody] CreateActivityRequest request,
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

            var command = new CreateActivityCommand
            {
                ActivityType = request.ActivityType,
                ActivityDuration = request.ActivityDuration,
                UserId = request.UserId
            };
            var result = await _mediator.Send(command, cancellationToken);
            return Created("http://{todo123.com}", result);
        }, cancellationToken);
    }
}