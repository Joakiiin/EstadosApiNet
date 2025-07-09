using EstadosApiNet.Contracts;
using EstadosApiNet.Models;
using EstadosApiNet.Repositories.IRepositories;

namespace EstadosApiNet.Services
{
    public class CodigoPostalService : ICodigoPostalService
    {
        private readonly ICodigoPostalRepository _repository;

        public CodigoPostalService(ICodigoPostalRepository repository)
        {
            _repository = repository;
        }

        public ApiResponse<object> BuscarPorCodigo(string codigo, bool agrupar = false)
        {
            if (string.IsNullOrWhiteSpace(codigo))
                return ErrorResponse(400, "El código postal no puede estar vacío");

            List<Asentamiento> asentamientos = _repository.GetByCodigo(codigo);
            if (!asentamientos.Any())
                return ErrorResponse(404, "No se encontraron registros");

            return agrupar
                ? AgruparRespuesta(asentamientos)
                : NoAgruparRespuesta(asentamientos);
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

        private ApiResponse<object> NoAgruparRespuesta(List<Asentamiento> asentamientos)
        {
            var response = asentamientos.Select(a => new CodigoPostalNoAgrupadoResponse
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

        private ApiResponse<object> AgruparRespuesta(List<Asentamiento> asentamientos)
        {
            Asentamiento primero = asentamientos.First();
            var response = new CodigoPostalResponse
            {
                cp = primero.d_codigo,
                asentamientos = asentamientos.Select(a => a.d_asenta).ToList(),
                tipos_asentamiento = asentamientos.Select(a => a.d_tipo_asenta).Distinct().ToList(),
                municipio = primero.D_mnpio,
                estado = primero.d_estado,
                ciudad = primero.d_ciudad,
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