using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Riva.DTO;
using Riva.Web.Services.IServices;

namespace Riva.Web.Controllers;

public class authController : Controller
{
    private readonly IAuthService _authService;
    private readonly IMapper _mapper;

    public authController(IAuthService authService, IMapper mapper)
    {
        _authService = authService;
        _mapper = mapper;
    }


    [HttpGet]
    public IActionResult Login()
    {
        return View();
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Login(LoginRequestDTO model)
    {
        try
        {
            var response = await _authService.LoginAsync<ApiResponse<LoginResponseDTO>>(model);

            if (response is not null && response.Success && response.Data is not null)
            {
                // token logic
            }

        }
        catch (Exception ex)
        {
            TempData["error"] = $"An error occurred: {ex.Message}";
        }

        return View();
    }


    [HttpGet]
    public IActionResult Register()
    {
        return View(new RegisterationRequestDTO
        {
            Email = string.Empty,
            Name = string.Empty,
            Password = string.Empty,
            Role = "Customer"
        });
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Register(RegisterationRequestDTO model)
    {
        try
        {
            var response = await _authService.RegisterAsync<ApiResponse<UserDTO>>(model);

            if (response is not null && response.Success && response.Data is not null)
            {
                // token logic
            }

        }
        catch (Exception ex)
        {
            TempData["error"] = $"An error occurred: {ex.Message}";
        }

        return View();
    }


    public IActionResult AccessDenied()
    {
        return View();
    }

    public async Task<IActionResult> Logout()
    {
        return View();
    }


}
