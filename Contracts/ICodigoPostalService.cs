using EstadosApiNet.Models;

namespace EstadosApiNet.Contracts
{
    public interface ICodigoPostalService
    {
        ApiResponse<object> BuscarPorCodigo(string codigo, bool agrupar = false);
    }
}