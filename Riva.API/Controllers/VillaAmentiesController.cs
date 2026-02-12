using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Riva.API.Data.Repository.IRepository;
using Riva.API.Models;
using Riva.DTO;
using System.Collections.Generic;

namespace Riva.API.Controllers;


[ApiController]
[Route("api/villaAmenties")]
public class VillaAmentiesController : ControllerBase
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;


    public VillaAmentiesController(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    [HttpGet]

    [ProducesResponseType(typeof(ApiResponse<IEnumerable<VillaAmentiesDTO>>), StatusCodes.Status200OK)]
    public async Task<ActionResult<ApiResponse<IEnumerable<VillaAmentiesDTO>>>> GetVillaAmenties()
    {

        var villaAmenties = await _unitOfWork.VillaAmenties.GetAllAsync();

        var villaAmentiesDtos = _mapper.Map<List<VillaAmentiesDTO>>(villaAmenties);

        var response = ApiResponse<IEnumerable<VillaAmentiesDTO>>.Ok(villaAmentiesDtos, "Villas Amenties retrieved successfully");

        return Ok(response);
    }

    [HttpGet("{id:int}")]
    [ProducesResponseType(typeof(ApiResponse<VillaAmentiesDTO>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status500InternalServerError)]

    public async Task<ActionResult<ApiResponse<VillaAmentiesDTO>>> GetVillaAmentiesById(int id)
    {
        try
        {
            if (id <= 0)
                return NotFound(ApiResponse<object>.NotFound("VillaAmenities ID must be greater than 0"));

            var villaAmenities = await _unitOfWork.VillaAmenties.GetAsync(u => u.Id == id);

            if (villaAmenities is null)
                return NotFound(ApiResponse<object>.NotFound($"VillaAmenities with ID {id} was not found"));

            var response = ApiResponse<VillaAmentiesDTO>.Ok(_mapper.Map<VillaAmentiesDTO>(villaAmenities), "VillaAmenities retrieved successfully");

            return Ok(response);
        }
        catch (Exception ex)
        {
            var errorResponse = ApiResponse<object>.Error(500, "An error occurred while retrieve the VillaAmenities : ", ex.Message);
            return StatusCode(500, errorResponse);
        }
    }

    [HttpPost]
    [ProducesResponseType(typeof(ApiResponse<VillaCreateDTO>), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status409Conflict)]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<ApiResponse<VillaAmentiesDTO>>> Create([FromBody]VillaCreateDTO model)
    {
        try
        {
            if (model is null)
                return BadRequest(ApiResponse<object>.BadRequest("VillaAmenities data is required"));

            var isExist = await _unitOfWork.VillaAmenties.GetAsync(u => u.Name.ToLower() == model.Name.ToLower());

            if (isExist is null)
                    return Conflict(ApiResponse<object>.Conflict($"VillaAmenities '{model.Name}' already exists"));

            var amenties = _mapper.Map<VillaAmenties>(model);

            await _unitOfWork.VillaAmenties.AddAsync(amenties);
            await _unitOfWork.Saveasync();

            var response = ApiResponse<VillaAmentiesDTO>.CreatedAt(_mapper.Map<VillaAmentiesDTO>(amenties), "Villa Amenities created successfully");

            return CreatedAtAction(nameof(Create), new { id = amenties.Id }, response);
        }
        catch(Exception ex)
        {
            var errorResponse = ApiResponse<object>.Error(500, "An error occurred while creating the VillaAmenities : ", ex.Message);
            return StatusCode(500, errorResponse);
        }
    }



}
