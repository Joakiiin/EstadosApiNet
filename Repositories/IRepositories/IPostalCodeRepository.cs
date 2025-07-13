using EstadosApiNet.Models;

namespace EstadosApiNet.Repositories.IRepositories
{
    public interface IPostalCodeRepository
    {
        List<Settlements> GetByCodigo(string code);
    }
}