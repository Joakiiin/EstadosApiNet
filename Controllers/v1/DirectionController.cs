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
        private readonly ICodigoPostalService _service;

        public DirectionController(ICodigoPostalService service)
        {
            _service = service;
        }
        [HttpGet("{codigo}")]
        public IActionResult Get(string codigo, [FromQuery] bool agrupar = false)
        {
            ApiResponse<object> result = _service.BuscarPorCodigo(codigo, agrupar);
            if (result.error)
            {
                return StatusCode(result.code_error, result);
            }
            return Ok(result);
        }
    }
}