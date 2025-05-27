using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CoreApp.API.Domain.Common;

public abstract class BaseEntity
{
  [Column("id")]
  [Key]
  public int Id { get; set; }
  [Column("createdAt")]
  [Required]
  public DateTimeOffset CreatedAt { get; set; }
  [Column("updatedAt")]

  public DateTimeOffset UpdatedAt { get; set; }
}
