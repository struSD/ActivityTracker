using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ActivityTracker.Contract.Http;
public class User
{
    [Column("id")]
    public int Id { get; init; }

    [Required]
    [MaxLength(255)]
    public string Name { get; init; }
    public ICollection<ActivityUser> ActivityUser { get; set; }
}