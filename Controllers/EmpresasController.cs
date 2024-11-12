using GerenciadorDeEmpresaWEB.Models;
using GerenciadorDeEmpresaWEB.Services;
using GerenciadorDeEmpresaWEB.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace GerenciadorDeEmpresaWEB.Controllers
{
    public class EmpresasController : Controller
    {
        private readonly IEmpresasServices _empresasService;
        private readonly ITipoEmpresaServices _tiposService;

        public EmpresasController(IEmpresasServices empresasService, ITipoEmpresaServices tiposService)
        {
            _empresasService = empresasService;
            _tiposService = tiposService;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<EmpresaViewModel>>> Index()
        {
            var result = await _empresasService.GetEmpresas();          

            if (result is null)
            {
                return View();
            }

            return View(result);
        }

        [HttpGet]
        public async Task<IActionResult> Create()
        {
            var tipoEmpresas = await _tiposService.GetTipoEmpresas();

            ViewBag.TipoEmpresas = new SelectList(tipoEmpresas, "Id", "Nome");

            return View();
        }



        [HttpPost]
        public async Task<ActionResult<EmpresaViewModel>> Create(EmpresaViewModel empresaViewModel) 
        {

            if (ModelState.IsValid)
            { 
                var result = await _empresasService.CriarEmpresa(empresaViewModel);

                if(result != null)
                    return RedirectToAction(nameof(Index));
            }
            var tipoEmpresas = await _tiposService.GetTipoEmpresas();

            // Mesmo que a lista seja vazia, ela será passada para o ViewBag sem problemas
            ViewBag.TipoEmpresas = new SelectList(tipoEmpresas, "Id", "Nome");
            ViewBag.Erro = "Erro ao criar Empresa";
            return View(empresaViewModel);
        }

        [HttpGet]
        public async Task<IActionResult> Details(int id)
        {
            var empresa = await _empresasService.GetEmpresaPorId(id);

            if (empresa is null)
                return View();

            var tipoEmpresa = await _tiposService.GetTipoEmpresasPorId(empresa.TipoEmpresaId);
            if (tipoEmpresa != null)
            {
                empresa.TipoEmpresaNome = tipoEmpresa.Nome;  
            }

            return View(empresa);
        }


        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            var result = await _empresasService.GetEmpresaPorId(id);

            if (result is null)
                return View("Error");

            var tipoEmpresas = await _tiposService.GetTipoEmpresas();

            // Mesmo que a lista seja vazia, ela será passada para o ViewBag sem problemas
            ViewBag.TipoEmpresas = new SelectList(tipoEmpresas, "Id", "Nome");

            return View(result);
        }
        [HttpPost]
        public async Task<ActionResult<EmpresaViewModel>> Edit(int id, EmpresaViewModel empresa)
        {
            if (ModelState.IsValid)
            {
                var result = await _empresasService.AtualizarEmpresa(id, empresa);

                if (result)
                    return RedirectToAction(nameof(Index));
            }
            var tipoEmpresas = await _tiposService.GetTipoEmpresas();

            // Mesmo que a lista seja vazia, ela será passada para o ViewBag sem problemas
            ViewBag.TipoEmpresas = new SelectList(tipoEmpresas, "Id", "Nome");
            return View(empresa);
        }
        [HttpGet]
        public async Task<ActionResult> Delete(int id)
        {
            var empresa = await _empresasService.GetEmpresaPorId(id);

            if (empresa == null)
                return View("Error");

            // Obter o tipo de empresa e definir o nome
            var tipoEmpresa = await _tiposService.GetTipoEmpresasPorId(empresa.TipoEmpresaId);
            if (tipoEmpresa != null)
            {
                empresa.TipoEmpresaNome = tipoEmpresa.Nome;
            }

            return View(empresa);
        }


        [HttpPost, ActionName("DeleteConfirmado")]
        public async Task<IActionResult> DeleteConfirm(int id)
        {
            var result = await _empresasService.DeletarEmpresa(id);

            if (result)
                return RedirectToAction("Index");

            return View();
        }

    }
}
