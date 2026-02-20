using Riva.DTO;
using Riva.Web.Services.IServices;
using Riva.Web.Shared;

namespace Riva.Web.Services;


public class VillaService : BaseService, IVillaService
{
    private readonly IHttpClientFactory _httpClientFactory;

    private const string API_EndPoint = "/api/villa";


    public VillaService(IHttpClientFactory httpClient) : base(httpClient)
    {
        _httpClientFactory = httpClient;
        
    }

    public Task<T?> CreateAsync<T>(VillaCreateDTO model, string token)
    {
        var apiRequest = new ApiRequest
        {
            httpMethod = Utils.HTTPmethods.POST,
            endpointURL = $"{API_EndPoint}",
            Data = model,
            token = token
        };

        return SendAsync<T>(apiRequest);
    }

    public Task<T?> DeleteAsync<T>(int id, string token)
    {
        var apiRequest = new ApiRequest
        {
            httpMethod = Utils.HTTPmethods.DELETE,
            endpointURL = $"{API_EndPoint}/{id}",
            token = token
        };

        return SendAsync<T>(apiRequest);
    }

    public Task<T?> GetAllAsync<T>(string token)
    {
        var apiRequest = new ApiRequest
        {
            httpMethod = Utils.HTTPmethods.GET,
            endpointURL = $"{API_EndPoint}",
            token = token
        };

        return SendAsync<T>(apiRequest);
    }

    public Task<T?> GetTAsync<T>(int id, string token)
    {
        var apiRequest = new ApiRequest
        {
            httpMethod = Utils.HTTPmethods.GET,
            endpointURL = $"{API_EndPoint}/{id}",
            token = token
        };

        return SendAsync<T>(apiRequest);
    }

    public Task<T?> UpdateAsync<T>(VillaUpdateDTO model, string token)
    {
        var apiRequest = new ApiRequest
        {
            httpMethod = Utils.HTTPmethods.PUT,
            endpointURL = $"{API_EndPoint}/{model.Id}",
            Data = model,
            token = token
        };

        return SendAsync<T>(apiRequest);
    }
}
