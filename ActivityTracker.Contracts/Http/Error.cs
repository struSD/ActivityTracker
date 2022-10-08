namespace ActivityTracker.Contracts.Http
{
    public enum ErrorCode
    {
        BadRequest = 40000,
        UserNotFound = 40401,
        ActivityNotFound = 40401,
        InternalServerError = 50000,
        DbFailureError = 50001
    }

    public class ErrorResponse
    {
        public ErrorCode Code { get; init; }
        public string Message { get; init; }
    }
}