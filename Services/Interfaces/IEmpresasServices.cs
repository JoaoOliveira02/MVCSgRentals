using GerenciadorDeEmpresaWEB.Models;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace GerenciadorDeEmpresaWEB.Services.Interfaces;

public interface IEmpresasServices
{
    Task<IEnumerable<EmpresaViewModel>> GetEmpresas();
    Task<EmpresaViewModel> GetEmpresaPorId(int id);
    Task<EmpresaViewModel> CriarEmpresa(EmpresaViewModel empresa);
    Task<bool> AtualizarEmpresa(int id, EmpresaViewModel empresa);
    Task<bool> DeletarEmpresa(int id);
}
