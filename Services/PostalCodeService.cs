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
        public ApiResponse<PostalCodeSearchResponse> SearchPostalCodes(string pattern, int? limit = null)
        {
            if (string.IsNullOrWhiteSpace(pattern) || pattern.Length < 2)
                return ErrorResponse<PostalCodeSearchResponse>(400, "El patrón de búsqueda debe tener al menos 2 caracteres");

            try
            {
                List<string> codes = _repository.SearchPostalCodes(pattern, limit);

                if (!codes.Any())
                    return ErrorResponse<PostalCodeSearchResponse>(404, "No se encontraron códigos postales que coincidan con el patrón");

                return new ApiResponse<PostalCodeSearchResponse>
                {
                    error = false,
                    code_error = 0,
                    error_message = null,
                    response = new PostalCodeSearchResponse { cp = codes }
                };
            }
            catch (Exception ex)
            {
                return ErrorResponse<PostalCodeSearchResponse>(500, ex.Message);
            }
        }
        public ApiResponse<EstadosResponse> GetUniqueEstados()
        {
            try
            {
                List<string> estados = _repository.GetUniqueEstados();

                return new ApiResponse<EstadosResponse>
                {
                    error = false,
                    code_error = 0,
                    error_message = null,
                    response = new EstadosResponse { Estados = estados }
                };
            }
            catch (Exception ex)
            {
                return ErrorResponse<EstadosResponse>(500, ex.Message);
            }
        }
        public ApiResponse<MunicipalitiesResponse> GetMunicipalitiesByState(string state)
        {
            if (string.IsNullOrWhiteSpace(state))
                return ErrorResponse<MunicipalitiesResponse>(400, "El estado no puede estar vacío");

            try
            {
                List<string> municipalities = _repository.GetMunicipalitiesByState(state);

                if (!municipalities.Any())
                    return ErrorResponse<MunicipalitiesResponse>(404, $"No se encontraron municipios para el estado: {state}");

                return new ApiResponse<MunicipalitiesResponse>
                {
                    error = false,
                    code_error = 0,
                    error_message = null,
                    response = new MunicipalitiesResponse { Municipalities = municipalities }
                };
            }
            catch (Exception ex)
            {
                return ErrorResponse<MunicipalitiesResponse>(500, ex.Message);
            }
        }
        public ApiResponse<PostalCodeSearchResponse> GetPostalCodesByMunicipality(string municipality)
        {
            if (string.IsNullOrWhiteSpace(municipality))
                return ErrorResponse<PostalCodeSearchResponse>(400, "El municipio no puede esar vacio");
            try
            {
                List<string> postalCodes = _repository.GetPostalCodesByMunicipalities(municipality);
                if (!postalCodes.Any())
                    return ErrorResponse<PostalCodeSearchResponse>(404, $"No se encontraron codigos postales para el municipio: {municipality}");
                    
                return new ApiResponse<PostalCodeSearchResponse>
                {
                    error = false,
                    code_error = 0,
                    error_message = null,
                    response = new PostalCodeSearchResponse { cp = postalCodes }
                };
            }
            catch (Exception ex)
            {
                return ErrorResponse<PostalCodeSearchResponse>(500, ex.Message);
            }
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
        private ApiResponse<T> ErrorResponse<T>(int code, string message)
        {
            return new ApiResponse<T>
            {
                error = true,
                code_error = code,
                error_message = message,
                response = default // Esto será null para tipos de referencia
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