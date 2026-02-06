using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Riva.API.Services.IServices;
using Riva.DTO;
using System.Linq.Expressions;

namespace Riva.API.Controllers
{
    [Route("api/Auth")]
    [ApiController]
    public class AuthController(IAuthService authService) : ControllerBase
    {
        private readonly IAuthService _authService = authService;


        [HttpPost]

        [ProducesResponseType(typeof(ApiResponse<UserDTO>), StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status409Conflict)]

        public async Task<ActionResult<ApiResponse<UserDTO>>> Register([FromBody] RegisterationRequestDTO registerationRequestDTO)
        {
            try
            {
                if (await _authService.IsEmailExistsAsync(registerationRequestDTO.Email))
                {
                    return Conflict(ApiResponse<object>.Conflict($"User with email '{registerationRequestDTO.Email}' already exists"));
                }

                var user =  await _authService.RegisterAsync(registerationRequestDTO);

                if (user is null)
                {
                    return BadRequest(ApiResponse<object>.BadRequest("Registration failed"));
                }

                var response = ApiResponse<UserDTO>.CreatedAt(user, "User registered successfully");
                
                return CreatedAtAction(nameof(Register), response);
            }
            catch (Exception ex)
            {
                var errorResponse = ApiResponse<object>.Error(500, "An error occurred during registration", ex.Message);
                return StatusCode(500, errorResponse);

            }
        }


        [HttpPost("login")]
        public async Task<ActionResult<ApiResponse<LoginResponseDTO>>> Login([FromBody] LoginRequestDTO loginRequestDTO)
        {
            var result = await _authService.LoginAsync(loginRequestDTO);

            if (result is null)
            {
                return BadRequest(ApiResponse<LoginResponseDTO>.BadRequest("Invalid email or password"));
            }

            var response = ApiResponse<LoginResponseDTO>.Ok(result, "Login successful");

            return Ok(response);
        }



    }
}
