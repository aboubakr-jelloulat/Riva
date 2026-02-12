using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Riva.API.Data.Repository.IRepository;
using Riva.API.Models;
using Riva.DTO;

[ApiController]
[Route("api/villa")]
[Authorize(Roles = "Admin, Customer")]
public class VillaController : ControllerBase
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public VillaController(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    [HttpGet]
    [ProducesResponseType(typeof(ApiResponse<IEnumerable<VillaDTO>>), StatusCodes.Status200OK)]
    public async Task<ActionResult<ApiResponse<IEnumerable<VillaDTO>>>> GetVillas()
    {
        var villas = await _unitOfWork.Villa.GetAllAsync();
        var villaDtos = _mapper.Map<List<VillaDTO>>(villas);

        var response = ApiResponse<IEnumerable<VillaDTO>>.Ok(villaDtos, "Villas retrieved successfully");
        return Ok(response);
    }

    [HttpGet("{id}")]
    [ProducesResponseType(typeof(ApiResponse<VillaDTO>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status404NotFound)]
    public async Task<ActionResult<ApiResponse<VillaDTO>>> GetVillaById(int id)
    {
        if (id <= 0)
            return BadRequest(ApiResponse<object>.BadRequest("Invalid villa id"));

        var villa = await _unitOfWork.Villa.GetAsync(u => u.Id == id);

        if (villa is null)
            return NotFound(ApiResponse<object>.NotFound("Villa not found"));

        var villaDto = _mapper.Map<VillaDTO>(villa);

        return Ok(ApiResponse<VillaDTO>.Ok(villaDto, "Villa retrieved successfully"));
    }

    [HttpPost]

    [ProducesResponseType(typeof(ApiResponse<VillaCreateDTO>), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status409Conflict)]
    public async Task<ActionResult<ApiResponse<VillaCreateDTO>>> CreateVilla(VillaCreateDTO villaDTO)
    {
        if (villaDTO is null)
            return BadRequest(ApiResponse<object>.BadRequest("Villa data is required"));

        // already exists in the database
        var exists = await _unitOfWork.Villa.GetAsync(u => u.Name.ToLower() == villaDTO.Name.ToLower());

        if (exists is not null)
            return Conflict(ApiResponse<object>.Conflict($"Villa '{villaDTO.Name}' already exists"));

        var villa = _mapper.Map<Villa>(villaDTO);

        await _unitOfWork.Villa.AddAsync(villa);
        await _unitOfWork.Saveasync();

        var response = ApiResponse<VillaCreateDTO>.CreatedAt(_mapper.Map<VillaCreateDTO>(villa), "Villa created successfully");
        return CreatedAtAction(nameof(CreateVilla), new { id = villa.Id }, response);
    }

    [HttpPut("{id:int}")]

    [ProducesResponseType(typeof(ApiResponse<VillaDTO>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status409Conflict)]
    public async Task<ActionResult<ApiResponse<VillaDTO>>> UpdateVilla(int id, VillaUpdateDTO villaDTO)
    {
        if (villaDTO is null)
            return BadRequest(ApiResponse<object>.BadRequest("Invalid request data"));

        if (id != villaDTO.Id)
        {
            return BadRequest(ApiResponse<object>.BadRequest("Villa Id is not match the Villa ID in request body"));
        }

        var villaFromDb = await _unitOfWork.Villa.GetAsync(u => u.Id == id, tracked: true);

        if (villaFromDb is null)
            return NotFound(ApiResponse<VillaDTO>.NotFound("Villa not found"));

        var duplicate = await _unitOfWork.Villa.GetAsync(u => u.Id != id && u.Name.ToLower() == villaDTO.Name.ToLower());

        if (duplicate is not null)
            return Conflict(ApiResponse<object>.Conflict($"Villa '{villaDTO.Name}' already exists"));

        _mapper.Map(villaDTO, villaFromDb);
        villaFromDb.UpdatedDate = DateTime.UtcNow;

        await _unitOfWork.Saveasync();

        var updatedDto = _mapper.Map<VillaDTO>(villaFromDb);

        var response = ApiResponse<VillaDTO>.Ok(updatedDto, "Villa updated successfully");
        return Ok(response);
    }

    [HttpDelete("{id}")]
    [Authorize("Admin")]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status204NoContent)]

    public async Task<IActionResult> DeleteVilla(int id)
    {
        var villa = await _unitOfWork.Villa.GetAsync(u => u.Id == id);

        if (villa is null)
            return NotFound(ApiResponse<object>.NotFound("Villa not found"));

        _unitOfWork.Villa.Remove(villa);
        await _unitOfWork.Saveasync();

        return NoContent();
    }
}
