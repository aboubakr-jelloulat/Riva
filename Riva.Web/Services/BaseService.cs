using Riva.DTO;
using Riva.Web.Services.IServices;
using Riva.Web.Shared;
using System.Text.Json;
using static Riva.Web.Shared.Utils;


namespace Riva.Web.Services;


public class BaseService : IBaseService
{
    public ApiResponse<object> ResponseModel { get ; set; }

    public IHttpClientFactory _httpClient { get; set; }

    private static readonly JsonSerializerOptions jsonOptions = new()
    {
        PropertyNameCaseInsensitive = true
        /* When converting JSON to a C# object ignore uppercase and lowercase differences in property names */
    };

    public BaseService(IHttpClientFactory httpClient)
    {
        ResponseModel = new();

        _httpClient = httpClient;
    }

    public async Task<T?> SendAsync<T>(ApiRequest apiRequest, bool withBearer = true)
    {
        try
        {
            var client = _httpClient.CreateClient("Riva.API");

            var message = new HttpRequestMessage
            {
                RequestUri = new Uri(apiRequest.endpointURL, UriKind.Relative),
                Method = apiRequest.httpMethod.ToHttpMethod()

                /*
                | Value                | Meaning                                                        |
                | -------------------- | -------------------------------------------------------------- |
                | `Absolute`           | The string must be a full URI: `https://example.com/api/villa` |
                | `Relative`           | The string is relative to a base URI: `/api/villa`             |
                
                This is a relative URI because it doesn’t have scheme (https) or host (localhost).

                 */
            };

            if (apiRequest.Data is not null)
            {
                message.Content = JsonContent.Create(apiRequest.Data, options: jsonOptions);
            }

            var apiResponse = await client.SendAsync(message);

            return (await apiResponse.Content.ReadFromJsonAsync<T>(jsonOptions));
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Unexpected Error: {ex.Message}");
            return default;
        }

    }


}

public static class HttpMethodExtensions
{
    public static HttpMethod ToHttpMethod(this HTTPmethods method)
    {
        return method switch
        {
            HTTPmethods.POST => HttpMethod.Post,
            HTTPmethods.PUT => HttpMethod.Put,
            HTTPmethods.DELETE => HttpMethod.Delete,
            _ => HttpMethod.Get
        };
    }
}
