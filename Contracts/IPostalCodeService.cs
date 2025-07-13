using EstadosApiNet.Models;

namespace EstadosApiNet.Contracts
{
    public interface IPostalCodeService
    {
        ApiResponse<object> SearchByPostalCode(string code, bool group = false);
    }
}