namespace GWTAI.Blazor.Client.Services.Contracts;

public interface IErrorFormattingService
{
  IEnumerable<string> GetFriendlyErrors(object? apiErrors);
}
