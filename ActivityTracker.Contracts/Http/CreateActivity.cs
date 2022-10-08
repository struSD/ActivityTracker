using System;
using System.ComponentModel.DataAnnotations;

using ActivityTracker.Contract.Database;

namespace ActivityTracker.Contract.Http;


public class CreateActivityRequest
{
    public string ActivityType { get; set; }
    public int ActivityDuration { get; set; }
    public int UserId { get; set; }
}

public class CreateActivityResponce
{
    public int Id { get; init; }
}