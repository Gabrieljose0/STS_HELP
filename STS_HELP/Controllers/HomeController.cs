using Microsoft.AspNetCore.Mvc;
using STS_HELP.Filters;
using STS_HELP.Helper;
using STS_HELP.Models;
using STS_HELP.Repositorio;
using System.Diagnostics;

namespace STS_HELP.Controllers
{
    [PaginaParaUsuariosLogados("Gestor", "Tecnico")]

    public class HomeController : VerificarSessaoController
    {

        private readonly IChamadoRepositorio _chamadoRepositorio;

        public HomeController(IChamadoRepositorio chamadoRepositorio, ISessao sessao) : base(sessao)
        {
            _chamadoRepositorio = chamadoRepositorio;
        }


        public IActionResult Index()
        {
            return View();
        }


        public IActionResult IndexGestor()
        {
            int total = _chamadoRepositorio.TotalChamados();
            int totalAberto = _chamadoRepositorio.TotalChamadosAberto();
            int totalEmAtendimento = _chamadoRepositorio.TotalChamadosEmAtendimento();
            int totalAtendimentoFinalizado = _chamadoRepositorio.TotalChamadosFinalizados();

            int TotalChamadosProblemaEquipamento = _chamadoRepositorio.TotalChamadosProblemaEquipamento();
            int TotalChamadosProblemaRede = _chamadoRepositorio.TotalChamadosProblemaRede();
            int TotalChamadosErroNoSistema = _chamadoRepositorio.TotalChamadosErroNoSistema();
            int TotalChamadosAcessoNegado = _chamadoRepositorio.TotalChamadosAcessoNegado();
            int TotalChamadosSolicitacaoDeEquipamento = _chamadoRepositorio.TotalChamadosSolicitacaoDeEquipamento();
            int TotalChamadosSolicitacaoDeSoftware = _chamadoRepositorio.TotalChamadosSolicitacaoDeSoftware();







            ViewBag.TotalDeChamados = total;
            ViewBag.TotalChamadosAberto = totalAberto;
            ViewBag.TotalDeChamadosEmAtendimento = totalEmAtendimento;
            ViewBag.TotalChamadosFinalizados = totalAtendimentoFinalizado;

            ViewBag.TotalChamadosProblemaEquipamento = TotalChamadosProblemaEquipamento;
            ViewBag.TotalChamadosProblemaRede = TotalChamadosProblemaRede;
            ViewBag.TotalChamadosErroNoSistema = TotalChamadosErroNoSistema;
            ViewBag.TotalChamadosAcessoNegado = TotalChamadosAcessoNegado;
            ViewBag.TotalChamadosSolicitacaoDeEquipamento = TotalChamadosSolicitacaoDeEquipamento;
            ViewBag.TotalChamadosSolicitacaoDeSoftware = TotalChamadosSolicitacaoDeSoftware;

            ViewBag.TotalAtendimentosIvanildo = _chamadoRepositorio.TotalAtendimentosPorTecnico(27);
            ViewBag.TotalAtendimentosViviane = _chamadoRepositorio.TotalAtendimentosPorTecnico(26);
            ViewBag.TotalAtendimentosEvelyn = _chamadoRepositorio.TotalAtendimentosPorTecnico(24);
            ViewBag.TotalAtendimentosAugusto = _chamadoRepositorio.TotalAtendimentosPorTecnico(30);




            return View("IndexGestor");
        }

       

        

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }


        
    }
}
