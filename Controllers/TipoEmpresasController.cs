using GerenciadorDeEmpresaWEB.Models;
using GerenciadorDeEmpresaWEB.Services;
using GerenciadorDeEmpresaWEB.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace GerenciadorDeEmpresaWEB.Controllers
{
    public class TipoEmpresasController : Controller
    {
        private readonly ITipoEmpresaServices _tipoEmpresasService;

        public TipoEmpresasController(ITipoEmpresaServices tipoEmpresasService)
        {
            _tipoEmpresasService = tipoEmpresasService;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<TipoEmpresaViewModel>>> Index()
        {
            var result = await _tipoEmpresasService.GetTipoEmpresas();

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
        public async Task<ActionResult<TipoEmpresaViewModel>> Create(TipoEmpresaViewModel tipoEmpresaViewModel) 
        {

            if (ModelState.IsValid)
            { 
                var result = await _tipoEmpresasService.CriarTipoEmpresas(tipoEmpresaViewModel);

                if(result != null)
                    return RedirectToAction(nameof(Index));
            }
           
            ViewBag.Erro = "Erro ao criar Tipo Empresa";
            return View(tipoEmpresaViewModel);
        }

        [HttpGet]
        public async Task<IActionResult> Details(int id)
        {
            var tipo = await _tipoEmpresasService.GetTipoEmpresasPorId(id);

            if (tipo is null)
                return View();

            return View(tipo);
        }


        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            var tipo = await _tipoEmpresasService.GetTipoEmpresasPorId(id);

            if (tipo is null)
                return View();

            return View(tipo);
        }
        [HttpPost]
        public async Task<ActionResult<TipoEmpresaViewModel>> Edit(int id, TipoEmpresaViewModel perfil)
        {
            if (ModelState.IsValid)
            {
                var result = await _tipoEmpresasService.AtualizarTipoEmpresas(id, perfil);

                if (result)
                    return RedirectToAction(nameof(Index));
            }

            return View(perfil);
        }
        [HttpGet]
        public async Task<ActionResult> Delete(int id)
        {
            var perfil = await _tipoEmpresasService.GetTipoEmpresasPorId(id);

            if (perfil is null)
                return View();

            return View(perfil);
        }


        [HttpPost, ActionName("DeleteConfirmado")]
        public async Task<IActionResult> DeleteConfirm(int id)
        {
            var result = await _tipoEmpresasService.DeletarTipoEmpresas(id);

            if (result)
                return RedirectToAction("Index");

            return View();
        }
    }
}
