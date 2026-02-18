using Riva.DTO;
using Riva.Web.Shared;

namespace Riva.Web.Services.IServices;


public interface IBaseService
{
    ApiResponse<object> ResponseModel { get; set; }

    Task<T> SendAsync<T>(ApiRequest apiRequest, bool withBearer = true);
}
