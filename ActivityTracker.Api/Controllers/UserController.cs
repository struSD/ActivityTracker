using System;
using System.ComponentModel.DataAnnotations;
using System.Threading;
using System.Threading.Tasks;

using ActivityTracker.Contract.Http;
using ActivityTracker.Domain.Commands;

using MediatR;

using Microsoft.AspNetCore.Mvc;

using Npgsql;

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
        try
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
        catch (InvalidOperationException ioe) when (ioe.InnerException is NpgsqlException)
        {
            var responce = new ErrorResponse
            {
                Code = ErrorCode.DbFailureError,
                Message = "DB fail"
            }; 
            return ToActionResult(responce);
        }
        catch (Exception)
        {
            var responce = new ErrorResponse
            {
                Code = ErrorCode.InternalServerError,
                Message = "Unhandled error"
            };
            return ToActionResult(responce);
        }
    }

    private IActionResult ToActionResult(ErrorResponse errorResponse)
    {
        return StatusCode((int)errorResponse.Code / 100, errorResponse);
    }

}