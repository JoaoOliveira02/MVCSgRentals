using GerenciadorDeEmpresaWEB.Models;

namespace GerenciadorDeEmpresaWEB.Services.Interfaces
{
    public interface IPerfilUsuarioServices
    {
        Task<IEnumerable<PerfilUsuarioViewModel>> GetPerfilUsuario();
        Task<PerfilUsuarioViewModel> GetPerfilUsuarioPorId(int id);
        Task<PerfilUsuarioViewModel> CriarPerfilUsuario(PerfilUsuarioViewModel perfilUsuario);
        Task<bool> AtualizarPerfilUsuario(int id, PerfilUsuarioViewModel perfilUsuario);
        Task<bool> DeletarPerfilUsuario(int id);
    }
}
