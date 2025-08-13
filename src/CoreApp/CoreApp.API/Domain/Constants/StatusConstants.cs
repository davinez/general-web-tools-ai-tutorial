using System.Text.Json.Serialization;

namespace CoreApp.API.Domain.Constants
{
  public static class StatusConstants
  {
    // Common Response Messages
    public const string SUCCESS = "Success";
    public const string FAILURE = "Failure";
    public const string ERROR = "Error";
    public const string INVALID_REQUEST = "Invalid request";
    public const string UNAUTHORIZED_ACCESS = "Unauthorized access";
    public const string RESOURCE_NOT_FOUND = "Resource not found";
    public const string INTERNAL_SERVER_ERROR = "Internal server error";

    [JsonConverter(typeof(JsonStringEnumConverter))]
    public enum PublicationStatus
    {
      NoPublished,
      InProgress,
      Completed,
      Failed
    }

    public const string VALIDATION_ERROR = "Validation error occurred";
    public const string RECORD_CREATED = "Record created successfully";
    public const string RECORD_UPDATED = "Record updated successfully";
    public const string RECORD_DELETED = "Record deleted successfully";
    public const string DUPLICATE_ENTRY = "Duplicate entry detected";
  }
}
