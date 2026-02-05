using AutoMapper;
using Riva.API.Data.Repository.IRepository;
using Riva.API.Models;
using Riva.API.Services.IServices;
using Riva.DTO;

namespace Riva.API.Services;

public class AuthService : IAuthService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public AuthService(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<bool> IsEmailExistsAsync(string email)
    {
        return await _unitOfWork.Users.AnyAsync(u => u.Email == email);
    }


    public Task<LoginResponseDTO?> LoginAsync(LoginRequestDTO loginRequestDTO)
    {
        throw new NotImplementedException();
    }

    public async Task<UserDTO?> RegisterAsync(RegisterationRequestDTO registerationRequestDTO)
    {
        try
        {

            User user = new()
            {
                Email = registerationRequestDTO.Email,
                Name = registerationRequestDTO.Name,
                Password = registerationRequestDTO.Password,

                Role = string.IsNullOrEmpty(registerationRequestDTO.Role)
                    ? "Customer" : registerationRequestDTO.Role,

                CreatedDate = DateTime.Now

            };

            await _unitOfWork.Users.AddAsync(user);
            await _unitOfWork.Saveasync();

            return _mapper.Map<UserDTO>(user);
        }
        catch(Exception ex)
        {
            throw new InvalidOperationException("An unexpected error occurred during user registration", ex);
        }
    }
}
