using EstadosApiNet.Models;

namespace EstadosApiNet.Contracts
{
    public interface IPostalCodeService
    {
        ApiResponse<object> SearchByPostalCode(string code, bool group = false);
        ApiResponse<PostalCodeSearchResponse> SearchPostalCodes(string pattern, int? limit = null);
        ApiResponse<EstadosResponse> GetUniqueEstados();
        ApiResponse<MunicipalitiesResponse> GetMunicipalitiesByState(string state);
        ApiResponse<PostalCodeSearchResponse> GetPostalCodesByMunicipality(string municipality);
    }
}