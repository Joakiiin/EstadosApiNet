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
        [HttpGet("search-cp/{pattern}")]
        public IActionResult SearchPostalCodes(string pattern, [FromQuery] int? limit = null)
        {
            ApiResponse<PostalCodeSearchResponse> result = _service.SearchPostalCodes(pattern, limit);
            return result.error
                ? StatusCode(result.code_error, result)
                : Ok(result);
        }
        [HttpGet("states")]
        public IActionResult GetUniqueEstados()
        {
            ApiResponse<EstadosResponse> result = _service.GetUniqueEstados();
            return result.error
                ? StatusCode(result.code_error, result)
                : Ok(result);
        }
        [HttpGet("get-municipalities-by-state/{state}")]
        public IActionResult GetMunicipalitiesByState(string state)
        {
            ApiResponse<MunicipalitiesResponse> result = _service.GetMunicipalitiesByState(state);
            return result.error
                ? StatusCode(result.code_error, result)
                : Ok(result);
        }
        [HttpGet("get-postal-code-by-municipalities/{municipality}")]
        public IActionResult GetPostalCodeByMunicipalities(string municipality)
        {
            ApiResponse<PostalCodeSearchResponse> result = _service.GetPostalCodesByMunicipality(municipality);
            return result.error
            ? StatusCode(result.code_error, result)
            : Ok(result);
        }
    }
}