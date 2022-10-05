using System;
using System.ComponentModel.DataAnnotations;

namespace ActivityTracker.Domain.Exceptions;

public class UserException : Exception
{
    public ErrorCode ErrorCode { get; }
    public UserException(ErrorCode errorCode) : this(errorCode, null)
    {

    }

    public UserException(ErrorCode errorCode, string message) : base(message)
    {
        ErrorCode = errorCode;
    }
}