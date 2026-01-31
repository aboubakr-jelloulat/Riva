using Microsoft.AspNetCore.Mvc;
using Riva.API.Data.Repository.IRepository;
using Riva.API.Models;
using Riva.DTO;
using System.Collections;
namespace Riva.API.Controllers;

[ApiController]
[Route("api/villa")]
public class VillaController : ControllerBase
{
    private readonly IUnitOfWork _unitOfWork;

    public VillaController(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    /*
     * ActionResult<T> : I will return data AND an HTTP status code
     */

    [HttpGet]
    public async Task<ActionResult<IEnumerable<Villa>>> GetVillas() => Ok(await _unitOfWork.Villa.GetAllAsync());

    [HttpGet("{id}")]
    public async Task<ActionResult<Villa>> GetVillaById(int id)
    {
        if (id <= 0)
            return BadRequest("Invalid villa id");

        var villa = await _unitOfWork.Villa.GetAsync(u => u.Id == id);

        if (villa is null)
            return NotFound();

        return Ok(villa);
    }

    [HttpPost]
    public async Task<ActionResult> AddVilla(VillaCreateDTO villaDTO)
    {
        if (villaDTO is null)
            return BadRequest("Villa Data is Required");

        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        Villa villa = new()
        {
            Name = villaDTO.Name,
            Details = villaDTO.Details,
            Rate = villaDTO.Rate,
            Sqft = villaDTO.Sqft,
            Occupancy = villaDTO.Occupancy,
            ImageUrl = villaDTO.ImageUrl
        };

        await _unitOfWork.Villa.AddAsync(villa);
        await _unitOfWork.Saveasync();

        return Ok(villa);
    }
}

