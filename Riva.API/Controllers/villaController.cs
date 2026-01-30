using Microsoft.AspNetCore.Mvc;
namespace Riva.API.Controllers;

[ApiController]
[Route("api/villa")]
public class villaController : ControllerBase
{
    [HttpGet] // GET api/villa
    public string GetAllVillas()
    {
        return "Get All Villas";
    }

    [HttpGet("{id:int}")] // GET api/villa/1
    public string GetVillaById(int id)
    {
        return $"Villa {id}";
    }

    [HttpGet("{id:int}/{Name}")]
    public string GetVillaByIdAndName(int id, string Name)
    {
        return $"Villa {id}, Name : {Name}";
    }


}
