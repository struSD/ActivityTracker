using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ActivityTracker.Contract.Http;

public class ActivityUser
{
    public int ActivityId { get; init; }

    [Required]
    [MaxLength(255)]
    public string ActivityType { get; init; }
    [Required]
    public DateTime? ActivityDateTime { get; init; }
    [Required]
    public int ActivityDuration { get; init; }
}