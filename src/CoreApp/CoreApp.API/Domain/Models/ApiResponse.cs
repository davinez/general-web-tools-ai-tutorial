namespace CoreApp.API.Domain.Models;

public class ApiResponse<T>
{
  public T? Data { get; set; }
}
