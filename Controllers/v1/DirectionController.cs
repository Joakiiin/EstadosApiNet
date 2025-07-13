using EstadosApiNet.Contracts;
using EstadosApiNet.Models;
using Microsoft.AspNetCore.Mvc;

namespace EstadosApiNet.Controllers.v1
{
    [ApiController]
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/[controller]")]
    public class DirectionController : ControllerBase
    {
        private readonly IPostalCodeService _service;

        public DirectionController(IPostalCodeService service)
        {
            _service = service;
        }
        [HttpGet("postal-code-info/{code}")]
        public IActionResult Get(string code, [FromQuery] bool group = false)
        {
            ApiResponse<object> result = _service.SearchByPostalCode(code, group);
            if (result.error)
            {
                return StatusCode(result.code_error, result);
            }
            return Ok(result);
        }
    }
}