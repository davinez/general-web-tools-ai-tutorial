using System.Net.Http.Headers;

namespace GWTAI.Blazor.Client.Shared.Extensions
{
  public static class HttpClientExtensions
  {
    public static void ConfigureGWTAIServiceClient(this HttpClient client)
    {
      client.BaseAddress = new Uri("http://localhost:8081/");
      client.DefaultRequestHeaders.Add("Custom-Header", "GoogleServiceHeaderValue");
    }

    public static void AttachDefaultAuthenticationHeader(this HttpClient client, string? token)
    {
      if (string.IsNullOrWhiteSpace(token))
      {
        if (client.DefaultRequestHeaders.Contains("Authorization"))
        {
          client.DefaultRequestHeaders.Remove("Authorization");
        }
        return;
      }

      client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Token", token);
    }

    public static void ConfigureGWTAI2ServiceClient(this HttpClient client)
    {
      client.BaseAddress = new Uri("https://otherapi.example.com/");
      client.DefaultRequestHeaders.Add("Custom-Header", "OtherServiceHeaderValue");
    }
  }

}
