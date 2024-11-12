using GerenciadorDeEmpresaWEB.Models;
using GerenciadorDeEmpresaWEB.Services;
using GerenciadorDeEmpresaWEB.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace GerenciadorDeEmpresaWEB.Controllers
{
    public class UsuariosController : Controller
    {
        private readonly IUsuarioServices _usuarioService;
        private readonly IEmpresasServices _empresasService;
        private readonly IPerfilUsuarioServices _perfilUsuariosService;


        public UsuariosController(IUsuarioServices usuarioService, IEmpresasServices empresasService, IPerfilUsuarioServices perfilUsuariosService)
        {
            _usuarioService = usuarioService;
            _empresasService = empresasService;
            _perfilUsuariosService = perfilUsuariosService;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<UsuarioViewModel>>> Index()
        {
            // Obter todos os usuários
            var usuarios = await _usuarioService.GetUsuarios();

            // Se não houver usuários, retorna a view vazia
            if (usuarios is null)
            {
                return View();
            }

            // Obter todas as empresas e perfis
            var empresas = await _empresasService.GetEmpresas();
            var perfis = await _perfilUsuariosService.GetPerfilUsuario(); // Supondo que você tenha esse serviço

            // Para cada usuário, pegar o nome da empresa e o nome do perfil
            foreach (var usuario in usuarios)
            {
                // Obter nome da empresa
                var empresa = empresas.FirstOrDefault(e => e.Id == usuario.EmpresaId);
                if (empresa != null)
                {
                    usuario.EmpresaNome = empresa.NomeFantasia;
                }

                // Obter nome do perfil
                var perfil = perfis.FirstOrDefault(p => p.Id == usuario.PerfilUsuarioId);
                if (perfil != null)
                {
                    usuario.PerfilUsuarioNome = perfil.Nome;  // Supondo que o perfil tenha uma propriedade 'Nome'
                }
            }

            // Retornar a lista de usuários com o nome da empresa e do perfil
            return View(usuarios);
        }


        [HttpGet]
        public async Task<IActionResult> Index1()
        {
            // Obter as empresas para o SelectList
            var empresas = await _empresasService.GetEmpresas();

            // Preencher o SelectList com as empresas
            ViewData["Empresas"] = new SelectList(empresas, "Id", "NomeFantasia");

            return View();  // Apenas exibe o SelectList de empresas
        }

        [HttpGet]
        public async Task<IActionResult> Index2()
        {
            // Obter os perfis para o SelectList
            var perfis = await _perfilUsuariosService.GetPerfilUsuario();

            // Preencher o SelectList com os perfis
            ViewData["Perfil"] = new SelectList(perfis, "Id", "Nome");

            // Retorna a View com o SelectList
            return View();  // Exibe a página de seleção de perfis
        }

        [HttpPost]
        public async Task<IActionResult> Index2Result(int perfilId)
        {
            // Obter os usuários por empresa
            var usuarios = await _usuarioService.GetUsuariosPorPerfil(perfilId);

            // Se não houver usuários, retorna a view vazia
            if (usuarios is null)
            {
                return View();
            }

            // Obter as empresas para o SelectList (caso seja necessário reter o SelectList na página)
            var perfis = await _perfilUsuariosService.GetPerfilUsuario();
            var empresas = await _empresasService.GetEmpresas();

            // Para cada usuário, pegar o nome da empresa e o nome do perfil
            foreach (var usuario in usuarios)
            {
                // Obter nome da empresa
                var empresa = empresas.FirstOrDefault(e => e.Id == usuario.EmpresaId);
                if (empresa != null)
                {
                    usuario.EmpresaNome = empresa.NomeFantasia;
                }

                // Obter nome do perfil
                var perfil = perfis.FirstOrDefault(p => p.Id == usuario.PerfilUsuarioId);
                if (perfil != null)
                {
                    usuario.PerfilUsuarioNome = perfil.Nome;  // Supondo que o perfil tenha uma propriedade 'Nome'
                }
            }

            ViewData["Perfil"] = new SelectList(perfis, "Id", "Nome", perfilId); // Mantém a empresa selecionada

            // Passar a lista de usuários para a View
            return View(usuarios);  // Passa os usuários para a View
        }


        // Método POST: Após selecionar uma empresa, exibe os usuários dessa empresa
        [HttpPost]
        public async Task<IActionResult> Index1Result(int empresaId)
        {
            // Obter os usuários por empresa
            var usuarios = await _usuarioService.GetUsuariosPorEmpresa(empresaId);
            // Se não houver usuários, retorna a view vazia
            if (usuarios is null)
            {
                return View();
            }

            // Obter as empresas para o SelectList (caso seja necessário reter o SelectList na página)
            var perfis = await _perfilUsuariosService.GetPerfilUsuario();
            var empresas = await _empresasService.GetEmpresas();

            // Para cada usuário, pegar o nome da empresa e o nome do perfil
            foreach (var usuario in usuarios)
            {
                // Obter nome da empresa
                var empresa = empresas.FirstOrDefault(e => e.Id == usuario.EmpresaId);
                if (empresa != null)
                {
                    usuario.EmpresaNome = empresa.NomeFantasia;
                }

                // Obter nome do perfil
                var perfil = perfis.FirstOrDefault(p => p.Id == usuario.PerfilUsuarioId);
                if (perfil != null)
                {
                    usuario.PerfilUsuarioNome = perfil.Nome;  // Supondo que o perfil tenha uma propriedade 'Nome'
                }
            }
            ViewData["Empresas"] = new SelectList(empresas, "Id", "NomeFantasia", empresaId); // Mantém a empresa selecionada

            // Passar a lista de usuários para a View
            return View(usuarios);  // Passa os usuários para a View
        }


        [HttpGet]
        public async Task<IActionResult> Create() 
        {
            var empresas = await _empresasService.GetEmpresas();
            var perfilUsuario = await _perfilUsuariosService.GetPerfilUsuario();

            // Mesmo que a lista seja vazia, ela será passada para o ViewBag sem problemas
            ViewBag.Empresas = new SelectList(empresas, "Id", "NomeFantasia");
            ViewBag.PerfilUsuario = new SelectList(perfilUsuario, "Id", "Nome");

            return View();
        }
        [HttpPost]
        public async Task<ActionResult<UsuarioViewModel>> Create(UsuarioViewModel usuario)
        {
            if (ModelState.IsValid)
            {
                // Verifica se o CPF já existe para a empresa
                var cpfExists = await _usuarioService.CPFExistsAsync(usuario.CPF, usuario.EmpresaId);

                if (cpfExists)
                {
                    // Adiciona uma mensagem de erro à ViewBag para exibir na tela
                    ViewBag.ErrorMessage = "Já existe um usuário com esse CPF na empresa.";

                    // Recarrega as listas de empresas e perfis para preencher o formulário novamente
                    var empresas = await _empresasService.GetEmpresas();
                    var perfilUsuario = await _perfilUsuariosService.GetPerfilUsuario();

                    ViewBag.Empresas = new SelectList(empresas, "Id", "NomeFantasia");
                    ViewBag.PerfilUsuario = new SelectList(perfilUsuario, "Id", "Nome");

                    // Retorna a view com a mensagem de erro e os dados inseridos pelo usuário
                    return View(usuario);
                }

                // Se o CPF não existir, tenta criar o usuário
                var result = await _usuarioService.CriarUsuarios(usuario);

                if (result != null)
                    return RedirectToAction(nameof(Index));
            }

            // Recarrega as listas se houver erro na model
            var empresasList = await _empresasService.GetEmpresas();
            var perfilUsuarioList = await _perfilUsuariosService.GetPerfilUsuario();

            ViewBag.Empresas = new SelectList(empresasList, "Id", "NomeFantasia");
            ViewBag.PerfilUsuario = new SelectList(perfilUsuarioList, "Id", "Nome");

            // Retorna a view com os dados inseridos e a mensagem de erro padrão
            ViewBag.Erro = "Erro ao criar o usuário.";
            return View(usuario);
        }



        [HttpGet]
        public async Task<IActionResult> Details(int id)
        {
            var usuario = await _usuarioService.GetUsuariosPorId(id);

            if (usuario is null)
                return View();

            var empresa = await _empresasService.GetEmpresaPorId(usuario.EmpresaId);
            if (empresa != null)
            {
                usuario.EmpresaNome = empresa.NomeFantasia;
            }

            var perfil = await _perfilUsuariosService.GetPerfilUsuarioPorId(usuario.EmpresaId);
            if (perfil != null)
            {
                usuario.PerfilUsuarioNome = perfil.Nome;
            }

            return View(usuario);
        }


        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            var usuario = await _usuarioService.GetUsuariosPorId(id);
            if (usuario == null)
            {
                return View("Error");
            }

            // Preencher os ViewBag com os dados de Perfis e Empresas
            ViewBag.PerfilUsuarios = new SelectList(await _perfilUsuariosService.GetPerfilUsuario(), "Id", "Nome");
            ViewBag.Empresas = new SelectList(await _empresasService.GetEmpresas(), "Id", "NomeFantasia");

            return View(usuario);
        }

        [HttpPost]
        public async Task<ActionResult<UsuarioViewModel>> Edit(int id, UsuarioViewModel usuario)
        {
            if (ModelState.IsValid)
            {
                var result = await _usuarioService.AtualizarUsuarios(id, usuario);

                if (result)
                    return RedirectToAction(nameof(Index));
            }
            // Preencher os ViewBag com os dados de Perfis e Empresas
            ViewBag.PerfilUsuarios = new SelectList(await _perfilUsuariosService.GetPerfilUsuario(), "Id", "Nome");
            ViewBag.Empresas = new SelectList(await _empresasService.GetEmpresas(), "Id", "NomeFantasia");

            return View(usuario);
        }
        [HttpGet]
        public async Task<ActionResult> Delete(int id)
        {
            var usuario = await _usuarioService.GetUsuariosPorId(id);

            if (usuario is null)
                return View();

            var empresa = await _empresasService.GetEmpresaPorId(usuario.EmpresaId);
            if (empresa != null)
            {
                usuario.EmpresaNome = empresa.NomeFantasia;
            }

            var perfil = await _perfilUsuariosService.GetPerfilUsuarioPorId(usuario.EmpresaId);
            if (perfil != null)
            {
                usuario.PerfilUsuarioNome = perfil.Nome;
            }

            return View(usuario);
        }


        [HttpPost, ActionName("DeleteConfirmado")]
        public async Task<IActionResult> DeleteConfirm(int id)
        {
            var result = await _usuarioService.DeletarUsuarios(id);

            if (result)
                return RedirectToAction("Index");

            return View();
        }
    }
}
