using AutoMapper;
using Microsoft.IdentityModel.Tokens;
using Riva.API.Data.Repository.IRepository;
using Riva.API.Models;
using Riva.API.Services.IServices;
using Riva.DTO;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Riva.API.Services;

public class AuthService : IAuthService
{
    private readonly IUnitOfWork    _unitOfWork;
    private readonly IMapper        _mapper;
    private readonly IConfiguration _configuration;

    public AuthService(IUnitOfWork unitOfWork, IMapper mapper, IConfiguration configuration)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
        _configuration = configuration;
    }

    public async Task<bool> IsEmailExistsAsync(string email)
    {
        return await _unitOfWork.Users.AnyAsync(u => u.Email == email);
    }


    public async Task<LoginResponseDTO?> LoginAsync(LoginRequestDTO loginRequestDTO)
    {
        try
        {
            if (loginRequestDTO is null)
                throw new ArgumentNullException(nameof(loginRequestDTO));

            var user = await _unitOfWork.Users.GetAsync(u => u.Email == loginRequestDTO.Email
                && u.Password == loginRequestDTO.Password);

            if (user is null) return null;

            var token = GenerateJwtToken(user);

            return new LoginResponseDTO
            {
                UserDTO = _mapper.Map<UserDTO>(user),
                Token = token
            };

        }
        catch(Exception ex)
        {
            throw new InvalidOperationException("An unexpected error occurred during user Login", ex);
        }
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

    private string GenerateJwtToken(User user)
    {
        var key = Encoding.ASCII.GetBytes(_configuration.GetSection("JWTSettings")["Secret"]);

        var tokenHandler = new JwtSecurityTokenHandler();

        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(new Claim[]
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(ClaimTypes.Email, user.Email),
                new Claim(ClaimTypes.Name, user.Name),
                new Claim(ClaimTypes.Role, user.Role)
            }),

            Expires = DateTime.UtcNow.AddDays(7),

            SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)

        };

        var token = tokenHandler.CreateToken(tokenDescriptor);

        return tokenHandler.WriteToken(token);
    }
}
