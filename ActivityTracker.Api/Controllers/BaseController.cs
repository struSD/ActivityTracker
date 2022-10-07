using System;
using System.ComponentModel.DataAnnotations;
using System.Threading;
using System.Threading.Tasks;

using ActivityTracker.Domain.Exceptions;

using Microsoft.AspNetCore.Mvc;

using Npgsql;

namespace ActivityTracker.Api.Controller;

public class BaseController : ControllerBase
{
    protected async Task<IActionResult> SafeExecute(Func<Task<IActionResult>> action, CancellationToken cancellationToken)
    {
        try
        {
            return await action();
        }
        catch (ActivityTrackerException ate)
        {
            var responce = new ErrorResponse
            {
                Code = ate.ErrorCode,
                Message = ate.Message
            };
            return ToActionResult(responce);
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
    protected IActionResult ToActionResult(ErrorResponse errorResponse)
    {
        return StatusCode((int)errorResponse.Code / 100, errorResponse);
    }
}