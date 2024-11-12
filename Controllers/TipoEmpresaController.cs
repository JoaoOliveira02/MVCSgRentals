using GerenciadorDeEmpresaWEB.Models;
using GerenciadorDeEmpresaWEB.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace GerenciadorDeEmpresaWEB.Controllers
{
    public class TipoEmpresaController : Controller
    {
        private readonly ITipoEmpresa _tipoEmpresasService;

        public TipoEmpresaController(ITipoEmpresa tipoEmpresasService)
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
           
            ViewBag.Erro = "Erro ao criar Empresa";
            return View(tipoEmpresaViewModel);
        }
    }
}
