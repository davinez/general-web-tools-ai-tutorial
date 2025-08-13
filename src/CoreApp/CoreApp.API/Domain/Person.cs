using System.Text.Json.Serialization;

namespace CoreApp.API.Domain;

public class Person
{
  [JsonIgnore]
  public int PersonId { get; init; }

  public string? Username { get; set; }

  public string? Email { get; set; }

  public string? Bio { get; set; }

  public string? Image { get; set; }
}
