using Riva.DTO;

namespace Riva.API.Services.IServices;

public interface IAuthService
{
    Task<UserDTO?> RegisterAsync(RegisterationRequestDTO registerationRequestDTO);

    Task<LoginResponseDTO?> LoginAsync(LoginRequestDTO loginRequestDTO);

    Task<bool> IsEmailExistsAsync(string email);

}
