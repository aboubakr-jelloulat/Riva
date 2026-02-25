using AutoMapper;
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



    public async Task<IActionResult> Index()
    {
        List<VillaDTO> villas = new();

        try
        {
            var response = await _villaService.GetAllAsync<ApiResponse<List<VillaDTO>>>("");

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


    public IActionResult Create()
    {
        return View();
    }


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
            var response = await _villaService.CreateAsync<ApiResponse<VillaDTO>>(model, "");

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

    public async Task<IActionResult> Delete(int id)
    {
        if (id <= 0)
        {
            TempData["error"] = "Invalid villa ID";
            return RedirectToAction(nameof(Index));
        }

        try
        {
            var response = await _villaService.GetTAsync<ApiResponse<VillaDTO>>(id, "");

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


    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Delete(VillaDTO model)
    {
        try
        {
            var response = await _villaService.DeleteAsync<ApiResponse<object>>(model.Id, "");

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
