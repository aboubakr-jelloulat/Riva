using Riva.API.Services.IServices;
using Riva.DTO;

namespace Riva.API.Services;

public class AuthService : IAuthService
{
    public Task<bool> IsEmailExistsAsync(string email)
    {
        throw new NotImplementedException();
    }

    public Task<LoginResponseDTO?> LoginAsync(LoginRequestDTO loginRequestDTO)
    {
        throw new NotImplementedException();
    }

    public Task<UserDTO?> RegisterAsync(RegisterationRequestDTO registerationRequestDTO)
    {
        throw new NotImplementedException();
    }
}
