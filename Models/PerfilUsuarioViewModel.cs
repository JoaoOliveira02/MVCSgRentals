using System.ComponentModel.DataAnnotations;

namespace GerenciadorDeEmpresaWEB.Models;
public class PerfilUsuarioViewModel
{
    public int Id { get; set; }

    [Required(ErrorMessage = "O nome é obrigatório.")]
    [StringLength(100, MinimumLength = 3, ErrorMessage = "O nome deve ter entre 3 e 100 caracteres.")]
    public string Nome { get; set; }
}
