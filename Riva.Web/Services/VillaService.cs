using Riva.DTO;
using Riva.Web.Services.IServices;
using Riva.Web.Shared;

namespace Riva.Web.Services;


public class VillaService : BaseService, IVillaService
{
    private const string Key = "ServiceUrls:RivaAPI";

    private readonly IHttpClientFactory _httpClientFactory;

    private readonly string _RivaUrl;

    private const string API_EndPoint = "/api/villa";


    public VillaService(IHttpClientFactory httpClient, IConfiguration configuration) : base(httpClient)
    {
        _httpClientFactory = httpClient;
        _RivaUrl = configuration.GetValue<string>(Key);
    }

    public Task<T?> CreateAsync<T>(VillaCreateDTO model, string token)
    {
        var apiRequest = new ApiRequest
        {
            httpMethod = Utils.HTTPmethods.POST,
            endpointURL = $"{_RivaUrl}{API_EndPoint}",
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
            endpointURL = $"{_RivaUrl}{API_EndPoint}/{id}",
            token = token
        };

        return SendAsync<T>(apiRequest);
    }

    public Task<T?> GetAllAsync<T>(string token)
    {
        var apiRequest = new ApiRequest
        {
            httpMethod = Utils.HTTPmethods.GET,
            endpointURL = $"{_RivaUrl}{API_EndPoint}",
            token = token
        };

        return SendAsync<T>(apiRequest);
    }

    public Task<T?> GetTAsync<T>(int id, string token)
    {
        var apiRequest = new ApiRequest
        {
            httpMethod = Utils.HTTPmethods.GET,
            endpointURL = $"{_RivaUrl}{API_EndPoint}/{id}",
            token = token
        };

        return SendAsync<T>(apiRequest);
    }

    public Task<T?> UpdateAsync<T>(VillaUpdateDTO model, string token)
    {
        var apiRequest = new ApiRequest
        {
            httpMethod = Utils.HTTPmethods.PUT,
            endpointURL = $"{_RivaUrl}{API_EndPoint}/{model.Id}",
            Data = model,
            token = token
        };

        return SendAsync<T>(apiRequest);
    }
}
