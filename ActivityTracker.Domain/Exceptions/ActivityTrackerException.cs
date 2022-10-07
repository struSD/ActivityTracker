using System;
using System.ComponentModel.DataAnnotations;

namespace ActivityTracker.Domain.Exceptions;

public class ActivityTrackerException : Exception
{
    public ErrorCode ErrorCode { get; }
    public ActivityTrackerException(ErrorCode errorCode) : this(errorCode, null)
    {

    }

    public ActivityTrackerException(ErrorCode errorCode, string message) : base(message)
    {
        ErrorCode = errorCode;
    }
}