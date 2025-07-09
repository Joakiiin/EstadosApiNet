using EstadosApiNet.Models;

namespace EstadosApiNet.Repositories.IRepositories
{
    public interface ICodigoPostalRepository
    {
        List<Asentamiento> GetByCodigo(string codigo);
    }
}