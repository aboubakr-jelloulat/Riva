using AutoMapper;
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
    private readonly IMapper _mapper;

    public VillaController(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
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

        //Villa villa = new()
        //{
        //    Name = villaDTO.Name,
        //    Details = villaDTO.Details,
        //    Rate = villaDTO.Rate,
        //    Sqft = villaDTO.Sqft,
        //    Occupancy = villaDTO.Occupancy,
        //    ImageUrl = villaDTO.ImageUrl
        //};

        Villa villa = _mapper.Map<Villa>(villaDTO);

        await _unitOfWork.Villa.AddAsync(villa);
        await _unitOfWork.Saveasync();

        return CreatedAtAction(nameof(AddVilla), new { id = villa.Id }, villa);
    }

    [HttpPut("{id:int}")]
    public async Task<ActionResult> UpdateVilla(int id, VillaUpdateDTO villaDTO)
    {
        if (villaDTO is null)
            return BadRequest("Villa Data is Required");
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        if (villaDTO.Id != id)
            return BadRequest("Villa Id is not match the Villa ID in request body");

        var villaFromDb = await _unitOfWork.Villa.GetAsync(u => u.Id == id, tracked: true);

        if (villaFromDb is null)
            return NotFound($"Villa with ID {id} not found");

        _mapper.Map(villaDTO, villaFromDb);

        villaFromDb.UpdatedDate = DateTime.Now;

        await _unitOfWork.Saveasync();

        return Ok(villaFromDb);
    }

    [HttpDelete("{id}")]
    public async Task<ActionResult> DeleteVilla(int id)
    {
        var villaFromDb = await _unitOfWork.Villa.GetAsync(u => u.Id == id);

        if (villaFromDb is null)
            return NotFound($"Villa with Id : {id} was not Found");

        await _unitOfWork.Villa.RemoveAsync(villaFromDb);
        await _unitOfWork.Saveasync();

        return NoContent();
    }
    

}

