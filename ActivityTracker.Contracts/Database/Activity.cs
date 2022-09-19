using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ActivityTracker.Contract.Database;

[Table("tbl_activity", Schema = "public")]
public class ActivityUser
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    [Column("activity_id")]
    public int ActivityId { get; set; }

    [Required]
    [MaxLength(255)]
    [Column("activity_type")]
    public string ActivityType { get; set; }

    [Required]
    [Column("activity_dateTime")]
    public DateTime ActivityDateTime { get; set; }

    [Required]
    [Column("activity_duration")]
    public int ActivityDuration { get; set; }
    
    [Column("user_id")]
    public int UserId { get; set; }
    public virtual User User { get; set; }
}