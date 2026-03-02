using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Riva.DTO;
using Riva.Web.Services.IServices;

namespace Riva.Web.Controllers;


public class VillaController : Controller
{
    private readonly IVillaService _villaService;
    private readonly IMapper _mapper;

    public VillaController(IVillaService villaService, IMapper mapper)
    {
        _villaService = villaService;
        _mapper = mapper;
    }


    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> Index()
    {
        List<VillaDTO> villas = new();

        try
        {
            var response = await _villaService.GetAllAsync<ApiResponse<List<VillaDTO>>>();

            if (response is not null && response.Success && response.Data is not null)
            {
                villas = response.Data;
            }
        }
        catch (Exception ex)
        {
            TempData["Error"] = $"An error occurred: {ex.Message}";
        }

        return View(villas);
    }

    [Authorize(Roles = "Admin")]
    public IActionResult Create()
    {
        return View();
    }

    [Authorize(Roles = "Admin")]
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(VillaCreateDTO model)
    {
        if (!ModelState.IsValid)
        {
            return View(model);
        }

        try
        {
            var response = await _villaService.CreateAsync<ApiResponse<VillaDTO>>(model);

            if (response is not null && response.Success && response.Data is not null)
            {
                TempData["success"] = "Villa created successfully";
                return RedirectToAction(nameof(Index));
            }

        }
        catch (Exception ex)
        {
            TempData["error"] = $"An error occurred: {ex.Message}";
        }

        return View(model);
    }

    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> Update(int id)
    {
        try
        {
            var response = await _villaService.GetTAsync<ApiResponse<VillaDTO>>(id);

            if (response is not null && response.Success && response.Data is not null)
            {
                return View(_mapper.Map<VillaUpdateDTO>(response.Data));
            }

        }
        catch (Exception ex)
        {
            TempData["error"] = $"An error occurred: {ex.Message}";
        }

        return View();
    }

    [Authorize(Roles = "Admin")]
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Update(VillaUpdateDTO model)
    {
        try
        {
            var response = await _villaService.UpdateAsync<ApiResponse<object>>(model);

            if (response is not null && response.Success)
            {
                TempData["success"] = "Villa Updated successfully";
            }
        }
        catch (Exception ex)
        {
            TempData["error"] = $"An error occurred: {ex.Message}";
        }

        return RedirectToAction(nameof(Index));
    }


    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> Delete(int id)
    {

        try
        {
            var response = await _villaService.GetTAsync<ApiResponse<VillaDTO>>(id);

            if (response is not null && response.Success && response.Data is not null)
            {
                return View(response.Data);
            }
        }
        catch (Exception ex)
        {
            TempData["error"] = $"An error occurred: {ex.Message}";
        }

        return View();
    }


    [Authorize(Roles = "Admin")]
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Delete(VillaDTO model)
    {
        try
        {
            var response = await _villaService.DeleteAsync<ApiResponse<object>>(model.Id);

            if (response is not null && response.Success)
            {
                TempData["success"] = "Villa deleted successfully";
            }
        }
        catch (Exception ex)
        {
            TempData["error"] = $"An error occurred: {ex.Message}";
        }

        return RedirectToAction(nameof(Index));
    }

}
