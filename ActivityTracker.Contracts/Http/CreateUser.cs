using System.ComponentModel.DataAnnotations;

using ActivityTracker.Contract.Database;

namespace ActivityTracker.Contract.Http;


public class CreateUserRequest
{
    [Required]
    [MaxLength(255)]
    public string Name { get; init; }
}

public class CreateUserResponce
{
    public int Id { get; init; }
}