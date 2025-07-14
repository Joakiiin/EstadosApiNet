using EstadosApiNet.Models;

namespace EstadosApiNet.Repositories.IRepositories
{
    public interface IPostalCodeRepository
    {
        List<Settlements> GetByCodigo(string code);
        List<string> SearchPostalCodes(string pattern, int? limit = null);
        List<string> GetUniqueEstados();
        List<string> GetMunicipalitiesByState(string state);
        List<string> GetPostalCodesByMunicipalities(string municipality);
    }
}