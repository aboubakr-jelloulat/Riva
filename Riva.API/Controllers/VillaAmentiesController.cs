using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Riva.API.Data.Repository.IRepository;
using Riva.DTO;

namespace Riva.API.Controllers;


[Route("api/villaAmenties")]
[ApiController]
public class VillaAmentiesController : ControllerBase
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;


    public VillaAmentiesController(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    //public Task<ActionResult<ApiResponse<IEnumerable<VillaAmentiesDTO>>>> GetVillaAmenties()
    //{


    //}

}
