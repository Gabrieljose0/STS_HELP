using Microsoft.AspNetCore.Mvc;
using STS_HELP.Helper;
using STS_HELP.Models;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace STS_HELP.Controllers
{
    public class loginController : Controller
    {

        private readonly IUsuariosRepositorio _usuariosRepositorio;
        private readonly ISessao _sessao;
        
        public loginController(IUsuariosRepositorio usuarioRepositorio, ISessao sessao)
        {
            _usuariosRepositorio = usuarioRepositorio;
            _sessao = sessao;
        }



        public IActionResult Index()
        {
            //Se o Usuario Estiver Logado, Redicionar para a Home

            if(_sessao.BuscarSessaoUsuario() != null)
            {
                return RedirectToAction("Index", "Home");
            }

            return View();
        }

        public IActionResult SairSistema()
        {
            _sessao.RemoverSessaoUsuario();

            return RedirectToAction("Index", "Login");
        }


        [HttpPost]
        public IActionResult LogarSistema(LoginModel loginModel)
        {
            try
            {
                if(ModelState.IsValid)
                {

                   UsuariosModel usuario = _usuariosRepositorio.BuscarLogin(loginModel.Email);

                    if(usuario != null )
                    {

                        _sessao.CriarSessaoUsuario(usuario);
                        if (usuario.SenhaValida(loginModel.Senha))
                        {
                            if(usuario.SituacaoUsuario == true)
                            {
                                //_sessao.CriarSessaoUsuario(usuario);
                                return RedirectToAction("Index", "Home");
                            }
                            else
                            {
                                TempData["MensagemErro"] = $"Usuario Inativo, Por favor entrar em contato com Gestor!";
                            }
                            
                        }


                        TempData["MensagemErro"] = $"E-mail ou Senha Incorretas. Tente Novamente";
                    }

                    TempData["MensagemErro"] = $"E-mail ou Senha Incorretas. Tente Novamente";
                    
                }

                return View("Index");

            }
            catch (Exception erro)
            {
                TempData["MensagemErro"] = $"Não Foi Possivel Efetuar Login. Tente Novamente, Detalhe do Erro: {erro.Message}";
                return RedirectToAction("Index");
            }
        }


    }
}
