using System.Threading;
using System.Threading.Tasks;

using ActivityTracker.Contract.Http;
using ActivityTracker.Domain.Commands;

using MediatR;

using Microsoft.AspNetCore.Mvc;

namespace ActivityTracker.Api.Controller;

[Route("api/activityTracker")]
public class ActivityTrackerController : ControllerBase
{
    private readonly IMediator _mediator;
    public ActivityTrackerController(IMediator mediator)
    {
        _mediator = mediator;
    }
    [HttpPut]
    public async Task<IActionResult> CreateActivityTracker([FromBody] CreateActivityTrackerRequest request,
        CancellationToken cancellationToken)
    {
        var command = new CreateActivityTrackerCommand
        {
            Name = request.Name
        };
        var result = await _mediator.Send(command, cancellationToken);
        var responce = new CreateActivityTrackerResponce
        {
            Id = result.ActivityTrackerId
        };
        return Created("http://todo.com", responce);
    }

}