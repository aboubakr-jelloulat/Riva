using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Riva.API.Data.Repository.IRepository;
using Riva.API.Models;
using Riva.DTO;
using System.Collections.Generic;

namespace Riva.API.Controllers;


[ApiController]
[Route("api/villaAmenities")]
//[Authorize(Roles = "Admin")]
public class VillaAmenitiesController : ControllerBase
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public VillaAmenitiesController(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    [HttpGet]
    [ProducesResponseType(typeof(ApiResponse<IEnumerable<VillaAmenitiesDTO>>), StatusCodes.Status200OK)]
    public async Task<ActionResult<ApiResponse<IEnumerable<VillaAmenitiesDTO>>>> GetVillaAmenities()
    {
        var villaAmenities = await _unitOfWork.VillaAmenities.GetAllAsync();

        var villaAmenitiesDtos = _mapper.Map<List<VillaAmenitiesDTO>>(villaAmenities);

        var response = ApiResponse<IEnumerable<VillaAmenitiesDTO>>.Ok(villaAmenitiesDtos, "Villa amenities retrieved successfully");

        return Ok(response);
    }

    [HttpGet("{id:int}")]
    [ProducesResponseType(typeof(ApiResponse<VillaAmenitiesDTO>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<ApiResponse<VillaAmenitiesDTO>>> GetVillaAmenitiesById(int id)
    {
        try
        {
            if (id <= 0)
                return BadRequest(ApiResponse<object>.BadRequest("VillaAmenities ID must be greater than 0"));

            var villaAmenities = await _unitOfWork.VillaAmenities.GetAsync(u => u.Id == id);

            if (villaAmenities is null)
                return NotFound(ApiResponse<object>.NotFound($"VillaAmenities with ID {id} was not found"));

            var response = ApiResponse<VillaAmenitiesDTO>.Ok(_mapper.Map<VillaAmenitiesDTO>(villaAmenities), "VillaAmenities retrieved successfully");

            return Ok(response);
        }
        catch (Exception ex)
        {
            var errorResponse = ApiResponse<object>.Error(500, "An error occurred while retrieving the VillaAmenities: ", ex.Message);
            return StatusCode(500, errorResponse);
        }
    }

    [HttpPost]
    [ProducesResponseType(typeof(ApiResponse<VillaAmenitiesDTO>), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status409Conflict)]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<ApiResponse<VillaAmenitiesDTO>>> Create([FromBody] VillaAmenitiesCreateDTO model)
    {
        try
        {
            if (model is null)
                return BadRequest(ApiResponse<object>.BadRequest("VillaAmenities data is required"));

            var isExist = await _unitOfWork.VillaAmenities.GetAsync(u => u.Name.ToLower() == model.Name.ToLower());

            if (isExist is not null)
                return Conflict(ApiResponse<object>.Conflict($"VillaAmenities '{model.Name}' already exists"));

            var amenities = _mapper.Map<VillaAmenities>(model);

            amenities.CreatedDate = DateTime.UtcNow;

            await _unitOfWork.VillaAmenities.AddAsync(amenities);

            await _unitOfWork.SaveAsync();

            var response = ApiResponse<VillaAmenitiesDTO>.CreatedAt(_mapper.Map<VillaAmenitiesDTO>(amenities), "VillaAmenities created successfully");

            return CreatedAtAction(nameof(GetVillaAmenitiesById), new { id = amenities.Id }, response);
        }
        catch (Exception ex)
        {
            var errorResponse = ApiResponse<object>.Error(500, "An error occurred while creating the VillaAmenities: ", ex.Message);
            return StatusCode(500, errorResponse);
        }
    }

    [HttpPut("{id:int}")]
    [ProducesResponseType(typeof(ApiResponse<VillaAmenitiesDTO>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<ApiResponse<VillaAmenitiesDTO>>> Update(int id, [FromBody] VillaAmenitiesUpdateDTO model)
    {
        try
        {
            if (model is null)
                return BadRequest(ApiResponse<object>.BadRequest("Invalid request data"));

            if (id != model.Id)
                return BadRequest(ApiResponse<object>.BadRequest("VillaAmenities ID does not match the model ID in request body"));

            var villaAmenitiesFromDb = await _unitOfWork.VillaAmenities.GetAsync(u => u.Id == id, tracked: true);

            if (villaAmenitiesFromDb is null)
                return NotFound(ApiResponse<object>.NotFound($"VillaAmenities with ID {id} was not found"));

            _mapper.Map(model, villaAmenitiesFromDb);

            villaAmenitiesFromDb.UpdatedDate = DateTime.UtcNow;

            await _unitOfWork.SaveAsync();

            var response = ApiResponse<VillaAmenitiesDTO>.Ok(_mapper.Map<VillaAmenitiesDTO>(villaAmenitiesFromDb), "VillaAmenities updated successfully");

            return Ok(response);
        }
        catch (Exception ex)
        {
            var errorResponse = ApiResponse<object>.Error(500, "An error occurred while updating the VillaAmenities: ", ex.Message);
            return StatusCode(500, errorResponse);
        }
    }

    [HttpDelete("{id:int}")]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<ApiResponse<object>>> Delete(int id)
    {
        try
        {
            var villaAmenities = await _unitOfWork.VillaAmenities.GetAsync(u => u.Id == id, tracked: true);

            if (villaAmenities is null)
                return NotFound(ApiResponse<object>.NotFound($"VillaAmenities with ID {id} was not found"));

            _unitOfWork.VillaAmenities.Remove(villaAmenities);

            await _unitOfWork.SaveAsync();

            return Ok(ApiResponse<object>.Ok(null, "VillaAmenities deleted successfully"));
        }
        catch (Exception ex)
        {
            var errorResponse = ApiResponse<object>.Error(500, "An error occurred while deleting the VillaAmenities: ", ex.Message);
            return StatusCode(500, errorResponse);
        }
    }
}
