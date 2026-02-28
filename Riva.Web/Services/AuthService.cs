
using Riva.DTO;
using Riva.Web.Services;
using Riva.Web.Services.IServices;
using Riva.Web.Shared;

public class AuthService : BaseService, IAuthService
{
    private const string API_EndPoint = "/api/Auth";

    public AuthService(IHttpClientFactory httpClient) : base(httpClient)
    {
        

    }

    public Task<T?> LoginAsync<T>(LoginRequestDTO model)
    {
        var apiRequest = new ApiRequest
        {
            httpMethod = Utils.HTTPmethods.POST,
            endpointURL = API_EndPoint + "/login",
            Data = model
        };

        return SendAsync<T>(apiRequest);
    }

    public Task<T?> RegisterAsync<T>(RegisterationRequestDTO model)
    {
        var apiRequest = new ApiRequest
        {
            httpMethod = Utils.HTTPmethods.POST,
            endpointURL = API_EndPoint + "/register",
            Data = model
        };

        return SendAsync<T>(apiRequest);
    }
}
