using GerenciadorDeEmpresaWEB.Models;

namespace GerenciadorDeEmpresaWEB.Services.Interfaces
{
    public interface ITipoEmpresaServices
    {
        Task<IEnumerable<TipoEmpresaViewModel>> GetTipoEmpresas();
        Task<TipoEmpresaViewModel> GetTipoEmpresasPorId(int id);
        Task<TipoEmpresaViewModel> CriarTipoEmpresas(TipoEmpresaViewModel tipoEmpresa);
        Task<bool> AtualizarTipoEmpresas(int id, TipoEmpresaViewModel tipoEmpresa);
        Task<bool> DeletarTipoEmpresas(int id);
    }
}
