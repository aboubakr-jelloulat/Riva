using static Riva.Web.Shared.Shared;

namespace Riva.Web.Shared;

public class ApiRequest
{
    public HTTPmethods httpMethod { get; set; } = HTTPmethods.GET;

    public string? endpointURL { get; set; }

    public object? Data { get; set; }

    public string? token { get; set; }

}

