using System.ComponentModel.DataAnnotations;

namespace ActivityTracker.Contract.Http;


public class CreateActivityTrackerRequest
{
    [Required]
    [MaxLength(255)]
    public string Name { get; set; }
}

public class CreateActivityTrackerResponce
{
    public int Id { get; set; }
}