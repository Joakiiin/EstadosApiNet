using EstadosApiNet.Contracts;
using EstadosApiNet.Models;
using EstadosApiNet.Repositories.IRepositories;

namespace EstadosApiNet.Services
{
    public class PostalCodeService : IPostalCodeService
    {
        private readonly IPostalCodeRepository _repository;

        public PostalCodeService(IPostalCodeRepository repository)
        {
            _repository = repository;
        }

        public ApiResponse<object> SearchByPostalCode(string code, bool group = false)
        {
            if (string.IsNullOrWhiteSpace(code))
                return ErrorResponse(400, "El código postal no puede estar vacío");

            List<Settlements> settlements = _repository.GetByCodigo(code);
            if (!settlements.Any())
                return ErrorResponse(404, "No se encontraron registros");

            return group
                ? GroupResponse(settlements)
                : NotGroupResponse(settlements);
        }

        private ApiResponse<object> ErrorResponse(int code, string message)
        {
            return new ApiResponse<object>
            {
                error = true,
                code_error = code,
                error_message = message,
                response = null
            };
        }

        private ApiResponse<object> NotGroupResponse(List<Settlements> settlements)
        {
            List<NonGroupedPostalCodeResponse> response = settlements.Select(a => new NonGroupedPostalCodeResponse
            {
                cp = a.d_codigo,
                asentamiento = a.d_asenta,
                tipo_asentamiento = a.d_tipo_asenta,
                municipio = a.D_mnpio,
                estado = a.d_estado,
                ciudad = a.d_ciudad,
                pais = "México"
            }).ToList();

            return new ApiResponse<object>
            {
                error = false,
                code_error = 0,
                error_message = null,
                response = response
            };
        }

        private ApiResponse<object> GroupResponse(List<Settlements> settlements)
        {
            Settlements first = settlements.First();
            PostalCodeResponse response = new PostalCodeResponse
            {
                cp = first.d_codigo,
                asentamientos = settlements.Select(a => a.d_asenta).ToList(),
                tipos_asentamiento = settlements.Select(a => a.d_tipo_asenta).Distinct().ToList(),
                municipio = first.D_mnpio,
                estado = first.d_estado,
                ciudad = first.d_ciudad,
                pais = "México"
            };

            return new ApiResponse<object>
            {
                error = false,
                code_error = 0,
                error_message = null,
                response = response
            };
        }
    }
}