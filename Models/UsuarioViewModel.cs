using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace GerenciadorDeEmpresaWEB.Models;
public class UsuarioViewModel
{
    public int Id { get; set; }

    [Required(ErrorMessage = "O nome é obrigatório.")]
    [StringLength(100, MinimumLength = 3, ErrorMessage = "O nome deve ter entre 5 e 100 caracteres.")]
    public string Nome { get; set; }

    [Required(ErrorMessage = "O CPF é obrigatório.")]
    [StringLength(14, ErrorMessage = "O CPF deve ter 14 caracteres.")]
    public string CPF { get; set; }

    [Required(ErrorMessage = "O Perfil de Usuário é obrigatório.")]
    public int PerfilUsuarioId { get; set; }
    [JsonIgnore]
    public string? PerfilUsuarioNome { get; set; }  // Não precisa de validação

    [JsonIgnore]
    public PerfilUsuarioViewModel? PerfilUsuario { get; set; }

    [Required(ErrorMessage = "A Empresa é obrigatória.")]
    public int EmpresaId { get; set; }
    [JsonIgnore]
    public string? EmpresaNome { get; set; }  // Não precisa de validação

    [JsonIgnore]
    public EmpresaViewModel? Empresa { get; set; }
}

