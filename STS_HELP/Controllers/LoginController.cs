using Microsoft.AspNetCore.Mvc;
using STS_HELP.Helper;
using STS_HELP.Models;
using Supabase;
using static System.Runtime.InteropServices.JavaScript.JSType;
using Supabase.Gotrue.Exceptions;
using System.Threading.Tasks;


namespace STS_HELP.Controllers
{
    public class loginController : Controller
    {

        private readonly IUsuariosRepositorio _usuariosRepositorio;
        private readonly ISessao _sessao;
        private readonly IConfiguration _configuration;

        public loginController(IUsuariosRepositorio usuarioRepositorio, ISessao sessao, IConfiguration configuration)
        {
            _usuariosRepositorio = usuarioRepositorio;
            _sessao = sessao;
            _configuration = configuration;
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
        public async Task<IActionResult> LogarSistema(LoginModel loginModel)
        {
            try
            {
                if(ModelState.IsValid)
                {

                    var supabaseUrl = _configuration["Supabase:Url"] + "/auth/v1";
                    var supabaseAnonKey = _configuration["Supabase:AnonKey"];


                    var publicClient = new Supabase.Gotrue.Client(new Supabase.Gotrue.ClientOptions
                    {
                        Url = supabaseUrl,
                        Headers = new Dictionary<string, string>
                        {
                            { "apikey", supabaseAnonKey } // Usa a chave ANON
                        }
                    });

                    
                    var session = await publicClient.SignIn(loginModel.Email, loginModel.Senha);

                    UsuariosModel usuario = _usuariosRepositorio.BuscarLogin(loginModel.Email);

                    if (usuario != null )
                    {

                        if (usuario.SituacaoUsuario == true)
                        {
                            _sessao.CriarSessaoUsuario(usuario);

                            if(usuario.TipoUsuario == "Gestor")
                            {
                                return RedirectToAction("IndexGestor", "Home");
                            }
                            else if(usuario.TipoUsuario == "Tecnico")
                            {
                                return RedirectToAction("Index", "Home");
                            }
                            else if (usuario.TipoUsuario == "Colaborador")
                            {
                                TempData["MensagemErro"] = $"Usuário Não Possui nivel de autorização de Acesso";
                                return View("Index");
                            }

                        }
                        else
                        {
                            TempData["MensagemErro"] = $"Usuario Inativo, Por favor entrar em contato com Gestor!";
                            return View("Index");
                        }

                    }
                    else
                    {
                        TempData["MensagemErro"] = $"Erro ao carregar perfil de usuário";
                        return View("Index");
                    }

                }

                return View("Index");

            }
            catch (GotrueException gotrueEx)
            {
                TempData["MensagemErro"] = $"Erro do Supabase: {gotrueEx.Message}";
                return View("Index");
            }
            catch (Exception erro)
            {
                TempData["MensagemErro"] = $"Não Foi Possivel Efetuar Login. Tente Novamente. Detalhe: {erro.Message}";
                return RedirectToAction("Index");
            }
        }


    }
}
