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
    public async Task<ActionResult<ApiResponse<IEnumerable<VillaDTO>>>> GetVillas()
    {
        var villas = await _unitOfWork.Villa.GetAllAsync();

        var villaDtos = _mapper.Map<List<VillaDTO>>(villas);

        var response = ApiResponse<IEnumerable<VillaDTO>>.Ok(villaDtos, "Villas retrieved successfully");

        return Ok(response);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<ApiResponse<VillaDTO>>> GetVillaById(int id)
    {
        if (id <= 0)
            return BadRequest(ApiResponse<VillaDTO>.BadRequest("Invalid villa id"));

        var villa = await _unitOfWork.Villa.GetAsync(u => u.Id == id);

        if (villa is null)
            return NotFound(ApiResponse<VillaDTO>.NotFound("Villa not found"));

        var villaDto = _mapper.Map<VillaDTO>(villa);
        return Ok(ApiResponse<VillaDTO>.Ok(villaDto, "Record retrieved successfully"));
    }



    [HttpPost]
    public async Task<ActionResult<VillaCreateDTO>> CreateVilla(VillaCreateDTO villaDTO)
    {
        if (villaDTO is null)
            return BadRequest("Villa Data is Required");

        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var isDuplicatVilla = await _unitOfWork.Villa.GetAsync(u => u.Name.ToLower() == villaDTO.Name.ToLower());
        if (isDuplicatVilla is not null)
            return Conflict($"Villa With the Name [ {villaDTO.Name} ] Already exists");
          

        Villa villa = _mapper.Map<Villa>(villaDTO);

        await _unitOfWork.Villa.AddAsync(villa);
        await _unitOfWork.Saveasync();

        return CreatedAtAction(nameof(CreateVilla), new { id = villa.Id }, _mapper.Map<VillaCreateDTO>(villa));
    }

    [HttpPut("{id:int}")]
    public async Task<ActionResult<VillaDTO>> UpdateVilla(int id, VillaUpdateDTO villaDTO)
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

        var isDuplicatVilla = await _unitOfWork.Villa.GetAsync(u => u.Id != id && u.Name.ToLower() == villaDTO.Name.ToLower());
        if (isDuplicatVilla is not null)
            return Conflict($"Villa With the Name [ {villaDTO.Name} ] Already exists");
            


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

