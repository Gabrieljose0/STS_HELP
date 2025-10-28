using Microsoft.AspNetCore.Mvc;
using STS_HELP.Filters;
using STS_HELP.Helper;
using STS_HELP.Models;

namespace STS_HELP.Controllers
{

    [PaginaParaUsuariosLogados("Gestor")]
    public class UsuariosController : VerificarSessaoController
    {

        private readonly IUsuariosRepositorio _usuariosRepositorio;
        private new readonly ISessao _sessao;

        public UsuariosController(IUsuariosRepositorio usuariosRepositorio, ISessao sessao) : base(sessao)
        {
            _usuariosRepositorio = usuariosRepositorio;
            _sessao = sessao;
        }




        //METODOS GET
        public IActionResult Index()
        {
            List<UsuariosModel> listaUsuarios = _usuariosRepositorio.ListarUsuarios();

            return View(listaUsuarios);
        }

        public IActionResult EditarUsuario(int id)
        {
            UsuariosModel exibeInfosUsuario =  _usuariosRepositorio.ExibeInfoUsuario(id);

            return View(exibeInfosUsuario);
        }

        public IActionResult InativarUsuario(int id)
        {
            UsuariosModel exibeInfosUsuario = _usuariosRepositorio.ExibeInfoUsuario(id);
            return View(exibeInfosUsuario);
        }


        public IActionResult CriarUsuario()
        {
            return View();
        }



        //METODOS POST
        [HttpPost]
        public IActionResult CriarUsuario(UsuariosModel usuarios)
        {
            if (ModelState.IsValid)
            {
                usuarios.SituacaoUsuario = true;

                _usuariosRepositorio.Adicionar(usuarios);

                return RedirectToAction("Index");
            }
            return View(usuarios);
        }


        [HttpPost]
        public IActionResult EditarUsuario(UsuariosModel usuarios)
        {
            if (ModelState.IsValid)
            {

                _usuariosRepositorio.EditaUsuario(usuarios);

                return RedirectToAction("Index");
            }
            return View(usuarios);
        }


        [HttpPost]
        public IActionResult Inativar(int id)
        {
            // Chamo repositório passando o ID
            _usuariosRepositorio.Inativar(id);

            return RedirectToAction("Index");
        }

    }
}
