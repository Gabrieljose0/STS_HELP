using Microsoft.AspNetCore.Mvc;
using Rotativa.AspNetCore;
using STS_HELP.Filters;
using STS_HELP.Helper;
using STS_HELP.Models;

namespace STS_HELP.Controllers
{
    [PaginaParaUsuariosLogados("Gestor", "Tecnico")]
    public class Chamados : VerificarSessaoController
    {
        private readonly IChamadoRepositorio _chamadoRepositorio;
        private new readonly ISessao _sessao;

        public Chamados(IChamadoRepositorio chamadoRepositorio, ISessao sessao) : base(sessao)
        {
            _chamadoRepositorio = chamadoRepositorio;
            _sessao = sessao;
        }


        //METODOS GET
        public IActionResult Index()
        {
            List<ChamadosModel> listaChamados = _chamadoRepositorio.ListarChamados();


            return View(listaChamados);

        }

        public IActionResult IndexChamadosGestor()
        {
            List<ChamadosModel> listaChamadosGestor = _chamadoRepositorio.ListarChamados();


            return View(listaChamadosGestor);

        }

        public IActionResult AceitarChamado(int id, int idTecnico)
        {

            return View();
        }

        public IActionResult FinalizarChamado()
        {
            return View();
        }

        public IActionResult CriarChamado()
        {
            return View();
        }


        //METODOS POST
        [HttpPost]
        public IActionResult CriarChamado(ChamadosModel chamados)
        {
            if (ModelState.IsValid)
            {
                chamados.Status.Id = 1;

                // Define a data de abertura como o momento da criação do chamado.
                chamados.dt_Abertura = DateTime.UtcNow;

                _chamadoRepositorio.Adicionar(chamados);

                return RedirectToAction("Index");
            }
            return View(chamados);
        }


        [HttpPost]
        public IActionResult AceitarEFinalizarChamado(int id)
        {


            UsuariosModel usuarioLogado = _sessao.BuscarSessaoUsuario();

            if (usuarioLogado == null)
            {
                return RedirectToAction("Index", "Login");
            }

            int idTecnicoLogado = usuarioLogado.Id;

            // Chamo repositório passando o ID
            _chamadoRepositorio.AceitarEFinalizarChamado(id, idTecnicoLogado);


            return RedirectToAction("Index");
        }







        public IActionResult GerarRelatorioPDF(string filtroTecnico, string filtroSolicitante, DateTime? filtroData)
        {
            // Chama o novo método do repositório passando os 3 filtros
            var chamadosFiltrados = _chamadoRepositorio.BuscarRelatorioPersonalizado(filtroTecnico, filtroSolicitante, filtroData);

            // Retorna para a View PDF
            return new ViewAsPdf("~/Views/Relatorios/RelatorioPDF.cshtml", chamadosFiltrados)
            {
                PageSize = Rotativa.AspNetCore.Options.Size.A4,
                PageOrientation = Rotativa.AspNetCore.Options.Orientation.Landscape,
                PageMargins = { Left = 10, Right = 10, Top = 10, Bottom = 10 }
            };
        }





    }

}
