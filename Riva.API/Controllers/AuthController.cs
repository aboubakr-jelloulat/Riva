using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Riva.DTO;

namespace Riva.API.Controllers
{
    [Route("api/Auth")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        [HttpGet]

        [ProducesResponseType(typeof(ApiResponse<UserDTO>), StatusCodes.Status200OK)]
        public async Task<ActionResult<ApiResponse<UserDTO>>> Register()
        {
            

            var response = ApiResponse<UserDTO>.Ok(null, "User Created Successfully");
            return Ok(response);
        }

    }
}
