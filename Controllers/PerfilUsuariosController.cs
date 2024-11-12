using GerenciadorDeEmpresaWEB.Models;
using GerenciadorDeEmpresaWEB.Services;
using GerenciadorDeEmpresaWEB.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace GerenciadorDeEmpresaWEB.Controllers
{
    public class PerfilUsuariosController : Controller
    {
        private readonly IPerfilUsuarioServices _perfilUsuarioService;

        public PerfilUsuariosController(IPerfilUsuarioServices perfilUsuarioService)
        {
            _perfilUsuarioService = perfilUsuarioService;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<PerfilUsuarioViewModel>>> Index()
        {
            var result = await _perfilUsuarioService.GetPerfilUsuario();

            if(result is null)
            {
                return View();
            }

            return View(result);
        }

        [HttpGet]
        public async Task<IActionResult> Create() 
        {
            return View();
        }

        [HttpPost]
        public async Task<ActionResult<PerfilUsuarioViewModel>> Create(PerfilUsuarioViewModel perfilUsuario) 
        {

            if (ModelState.IsValid)
            { 
                var result = await _perfilUsuarioService.CriarPerfilUsuario(perfilUsuario);

                if(result != null)
                    return RedirectToAction(nameof(Index));
            }          

            ViewBag.Erro = "Erro ao criar Empresa";
            return View(perfilUsuario);
        }

        [HttpGet]
        public async Task<IActionResult> Details(int id)
        {
            var perfil = await _perfilUsuarioService.GetPerfilUsuarioPorId(id);

            if (perfil is null)
                return View();

            return View(perfil);
        }


        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            var perfil = await _perfilUsuarioService.GetPerfilUsuarioPorId(id);

            if (perfil is null)
                return View();

            return View(perfil);
        }
        [HttpPost]
        public async Task<ActionResult<PerfilUsuarioViewModel>> Edit(int id, PerfilUsuarioViewModel perfil)
        {
            if (ModelState.IsValid)
            {
                var result = await _perfilUsuarioService.AtualizarPerfilUsuario(id, perfil);

                if (result)
                    return RedirectToAction(nameof(Index));
            }
          
            return View(perfil);
        }
        [HttpGet]
        public async Task<ActionResult> Delete(int id)
        {
            var perfil = await _perfilUsuarioService.GetPerfilUsuarioPorId(id);

            if (perfil is null)
                return View();

            return View(perfil);
        }


        [HttpPost, ActionName("DeleteConfirmado")]
        public async Task<IActionResult> DeleteConfirm(int id)
        {
            var result = await _perfilUsuarioService.DeletarPerfilUsuario(id);

            if (result)
                return RedirectToAction("Index");

            return View();
        }
    }
}
