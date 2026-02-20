using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Riva.DTO;
using Riva.Web.Models;
using Riva.Web.Services.IServices;
using System.Diagnostics;

namespace Riva.Web.Controllers
{
    public class HomeController : Controller
    {
        private readonly IVillaService _villaService;

        public HomeController(IVillaService villaService, IMapper mapper)
        {
            _villaService = villaService;
            _mapper = mapper;
        }

        private readonly IMapper _mapper;

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

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
