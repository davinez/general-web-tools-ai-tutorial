using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CoreApp.API.Domain;

/// <summary>
/// Maps to the JobEvents table in the database.
/// Stores a historical record of status changes for a specific background job.
/// </summary>
[Table("JobEvents")]
public class JobEvent
{
  /// <summary>
  /// The unique identifier for the job this event belongs to.
  /// Primary Key, Foreign key to the main Jobs table.
  /// </summary>
  [Key]
  public Guid JobEventId { get; set; }

  /// <summary>
  /// The identifier for the user who initiated the job.
  /// Used to route webhook notifications.
  /// </summary>
  [Required]
  [StringLength(450)]
  public required string UserId { get; set; }

  /// <summary>
  /// The UTC timestamp when the event was recorded.
  /// </summary>
  [Required]
  public DateTime EventTimestamp { get; set; }

  /// <summary>
  /// The status of the job at the time of the event.
  /// Stored as a string in the database but mapped to the JobStatus enum in the code.
  /// </summary>
  [Required]
  [StringLength(50)]
  public required string Status { get; set; }

  /// <summary>
  /// Serialized object depending on workflow that triggered event.
  /// </summary>
  [Required]
  public required string Content { get; set; }

  /// <summary>
  /// Mapped to enum representing the workflow that triggered this event.
  /// </summary>
  [Required]
  public required string Workflow { get; set; }
}
