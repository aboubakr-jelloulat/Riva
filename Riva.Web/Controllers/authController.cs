using AutoMapper;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using Riva.DTO;
using Riva.Web.Services.IServices;
using Riva.Web.Shared;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using static System.Runtime.InteropServices.JavaScript.JSType;

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
                var mdl = response.Data;

                var handler = new JwtSecurityTokenHandler();
                var jwt = handler.ReadJwtToken(mdl.Token);

                var identity = new ClaimsIdentity(CookieAuthenticationDefaults.AuthenticationScheme);
                identity.AddClaim(new Claim(ClaimTypes.Name, jwt.Claims.FirstOrDefault(u => u.Type == "email").Value));
                identity.AddClaim(new Claim(ClaimTypes.Role, jwt.Claims.FirstOrDefault(u => u.Type == "role").Value));

                var principal = new ClaimsPrincipal(identity);

                await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal);

                HttpContext.Session.SetString(Utils.SessionAccessToken, mdl.Token);

                return RedirectToAction("Index", "Home");
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
                TempData["success"] = "Registration successful! Please login with your credentials.";
                return RedirectToAction("Login");
            }
            else
            {
                TempData["error"] = response?.Message ?? "Registration failed. Please try again.";
                return View(model);
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
        await HttpContext.SignOutAsync();

        return RedirectToAction("Index", "Home");
    }


}
