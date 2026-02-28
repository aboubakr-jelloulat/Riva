using Riva.DTO;

namespace Riva.Web.Services.IServices
{
    public interface IAuthService
    {
        Task<T?> RegisterAsync<T>(RegisterationRequestDTO model);
        Task<T?> LoginAsync<T>(LoginRequestDTO model);

    }
}
