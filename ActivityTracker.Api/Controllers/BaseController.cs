using System;
using System.Threading.Tasks;

using ActivityTracker.Contracts.Http;
using ActivityTracker.Domain.Exceptions;

using Microsoft.AspNetCore.Mvc;

using Npgsql;

namespace ActivityTracker.Api.Controllers
{
    public class BaseController : ControllerBase
    {
        protected async Task<IActionResult> SafeExecute(Func<Task<IActionResult>> action)
        {
            try
            {
                return await action();
            }
            catch (ActivityTrackerException ate)
            {
                ErrorResponse responce = new()
                {
                    Code = ate.ErrorCode,
                    Message = ate.Message
                };
                return ToActionResult(responce);
            }
            catch (InvalidOperationException ioe) when (ioe.InnerException is NpgsqlException)
            {
                ErrorResponse responce = new()
                {
                    Code = ErrorCode.DbFailureError,
                    Message = "DB fail"
                };
                return ToActionResult(responce);
            }
            catch (Exception)
            {
                ErrorResponse responce = new()
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
}