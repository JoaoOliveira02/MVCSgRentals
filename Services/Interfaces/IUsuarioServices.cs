using GerenciadorDeEmpresaWEB.Models;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace GerenciadorDeEmpresaWEB.Services.Interfaces;

public interface IUsuarioServices
{
    Task<IEnumerable<UsuarioViewModel>> GetUsuarios();
    Task<UsuarioViewModel> GetUsuariosPorId(int id);
    Task<IEnumerable<UsuarioViewModel>> GetUsuariosPorEmpresa(int empresaId);
    Task<IEnumerable<UsuarioViewModel>> GetUsuariosPorPerfil(int perfilId);
    Task<bool> CPFExistsAsync(string cpf, int empresaId);
    Task<UsuarioViewModel> CriarUsuarios(UsuarioViewModel usuario);
    Task<bool> AtualizarUsuarios(int id, UsuarioViewModel usuario);
    Task<bool> DeletarUsuarios(int id);
}
