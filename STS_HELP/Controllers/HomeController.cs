using Microsoft.AspNetCore.Mvc;
using STS_HELP.Filters;
using STS_HELP.Helper;
using STS_HELP.Models;
using System.Diagnostics;

namespace STS_HELP.Controllers
{
    [PaginaParaUsuariosLogados("Gestor", "Tecnico")]

    public class HomeController : VerificarSessaoController
    {
        public HomeController(ISessao sessao) : base(sessao)
        {
        }

        //private readonly ILogger<HomeController> _logger;

        //public HomeController(ILogger<HomeController> logger)
        //{
        //    _logger = logger;
        //}

        public IActionResult Index()
        {
            return View();
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
