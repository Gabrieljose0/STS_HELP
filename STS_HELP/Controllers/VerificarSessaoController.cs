using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using STS_HELP.Helper;

namespace STS_HELP.Controllers
{
    public class VerificarSessaoController : Controller
    {
        protected readonly ISessao _sessao;

        // 2. Crie um construtor que recebe a sessão
        public VerificarSessaoController(ISessao sessao)
        {
            _sessao = sessao;
        }

        public override void OnActionExecuting(ActionExecutingContext context)
        {
            var usuario = _sessao.BuscarSessaoUsuario();

            // Caso 1: Não há sessão (ninguém logado)
            if (usuario == null)
            {
                // Manda para a tela de Login
                context.Result = new RedirectToRouteResult(new RouteValueDictionary { { "controller", "Login" }, { "action", "Index" } });
            }
            // Caso 2: Há sessão, mas o usuário está INATIVO
            else if (usuario.SituacaoUsuario == false)
            {

                _sessao.RemoverSessaoUsuario();

                TempData["MensagemErro"] = "Seu usuário foi desativado. Por favor, contate o gestor.";

                // Manda para a tela de Login
                context.Result = new RedirectToRouteResult(new RouteValueDictionary { { "controller", "Login" }, { "action", "Index" } });
            }

            // o código continua normalmente.
            base.OnActionExecuting(context);
        }
    }
}
