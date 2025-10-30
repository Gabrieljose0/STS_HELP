using STS_HELP.Data;
using STS_HELP.Models;
using Supabase.Gotrue;
using Supabase.Gotrue.Interfaces;
using System.Threading.Tasks;
using System.Net.Http;
using System.Text;
using System.Text.Json;

namespace STS_HELP.Repositorio
{
    public class UsuariosRepositorio : IUsuariosRepositorio
    {

        private readonly BancoContext _bancoContext;
        private readonly Supabase.Gotrue.Client _gotrueClient;
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly IConfiguration _configuration;

        public UsuariosRepositorio(BancoContext bancoContext, Supabase.Gotrue.Client gotrueClient, IHttpClientFactory httpClientFactory, IConfiguration configuration)
        {
            _bancoContext = bancoContext;
            _gotrueClient = gotrueClient;
            _httpClientFactory = httpClientFactory;
            _configuration = configuration;
        }

        public List<UsuariosModel> ListarUsuarios()
        {
            return _bancoContext.Usuarios.OrderBy(u => u.Id).ToList();
        }


        public async Task<UsuariosModel> Adicionar(UsuariosModel usuarios)
        {
            try
            {
                // 1. CHAMA A API ADMIN (Sintaxe correta para Gotrue puro)
                var options = new SignUpOptions
                {
                    Data = new Dictionary<string, object>
                        {
                            { "nome", usuarios.Nome },
                            { "tipo_usuario", usuarios.TipoUsuario }
                        }
                };

                // Usamos o 'SignUp' com a service_key, que o torna um 'Admin Create'
                var user = await _gotrueClient.SignUp(
                    usuarios.Email,
                    usuarios.SenhaParaCadastro,
                    options
                );

                if (user?.User?.Id == null)
                {
                    throw new Exception("Falha ao criar usuário na autenticação.");
                }

                var authUuid = Guid.Parse(user.User.Id);
                var novoUsuarioDoBanco = _bancoContext.Usuarios
                                            .FirstOrDefault(u => u.AuthId == authUuid);

                if (novoUsuarioDoBanco == null)
                {
                    throw new Exception("Usuário criado, mas não encontrado. Verifique o gatilho.");
                }

                return novoUsuarioDoBanco;
            }
            catch (Exception ex)
            {
                throw new Exception($"Erro ao criar usuário: {ex.Message}", ex);
            }
        }

        public UsuariosModel ExibeInfoUsuario(int id)
        {
            return _bancoContext.Usuarios.FirstOrDefault(x => x.Id == id);
        }

        public async Task<UsuariosModel> EditaUsuario(UsuariosModel usuarios)
        {
            // 1. Encontra o perfil no seu banco de dados
            UsuariosModel usuarioDB = ExibeInfoUsuario(usuarios.Id);
            if (usuarioDB == null) throw new Exception("Houve um Erro na Edição do Cadastro do Usuário");

            // 2. Prepara um payload dinâmico para a API de Autenticação
            var authApiPayload = new Dictionary<string, object>();

            // 3. --- LÓGICA DA NOVA SENHA ---
            // Se o admin digitou uma nova senha, adiciona ao payload
            if (!string.IsNullOrEmpty(usuarios.SenhaParaCadastro))
            {
                authApiPayload["password"] = usuarios.SenhaParaCadastro;
            }

            // 4. --- LÓGICA DO NOVO EMAIL ---
            // Se o email do formulário é diferente do email no banco, adiciona ao payload
            if (usuarioDB.Email != usuarios.Email)
            {
                authApiPayload["email"] = usuarios.Email;
            }

            
            usuarioDB.Nome = usuarios.Nome;
            usuarioDB.Email = usuarios.Email; // Atualiza o email local também
            usuarioDB.TipoUsuario = usuarios.TipoUsuario;
            _bancoContext.Usuarios.Update(usuarioDB);

          
            if (authApiPayload.Count > 0)
            {
                if (usuarioDB.AuthId == null)
                {
                    throw new Exception("Este usuário não pode ter os dados de autenticação alterados (sem AuthId).");
                }

                // Pega a URL base e a ServiceKey da configuração
                var supabaseUrl = _configuration["Supabase:Url"];
                var supabaseServiceKey = _configuration["Supabase:ServiceKey"];
                var userId = usuarioDB.AuthId.ToString();

                // Monta a URL da API Admin (com o /auth/v1 que descobrimos)
                var requestUrl = $"{supabaseUrl}/auth/v1/admin/users/{userId}";

                // Cria o corpo da requisição (payload JSON)
                var jsonPayload = JsonSerializer.Serialize(authApiPayload);
                var httpContent = new StringContent(jsonPayload, Encoding.UTF8, "application/json");

                // Cria o cliente HTTP
                var httpClient = _httpClientFactory.CreateClient();
                var request = new HttpRequestMessage(HttpMethod.Put, requestUrl);

                // Adiciona os Headers OBRIGATÓRIOS de Admin
                request.Headers.Add("apikey", supabaseServiceKey);
                request.Headers.Add("Authorization", $"Bearer {supabaseServiceKey}");
                request.Content = httpContent;

                // Envia a requisição
                var response = await httpClient.SendAsync(request);

                if (!response.IsSuccessStatusCode)
                {
                    var errorContent = await response.Content.ReadAsStringAsync();
                    throw new Exception($"Erro ao atualizar dados no Supabase Auth: {response.StatusCode} - {errorContent}");
                }
            }

            // 7. Salva as mudanças do perfil local (Nome/Email/Tipo)
            await _bancoContext.SaveChangesAsync();
            return usuarioDB;
        }


        public UsuariosModel Inativar(int id)
        {
            
            UsuariosModel usuarioDB = ExibeInfoUsuario(id);

            if (usuarioDB == null)
            {
                throw new Exception("Erro ao Inativar Usuário: ID não encontrado.");
            }

            if(usuarioDB.SituacaoUsuario == false)
            {
                usuarioDB.SituacaoUsuario = true;
            }
            else
            {
                usuarioDB.SituacaoUsuario = false;
            }

            _bancoContext.Usuarios.Update(usuarioDB);
            _bancoContext.SaveChanges();

            return usuarioDB;
        }


        public UsuariosModel BuscarLogin(string email)
        {
            return _bancoContext.Usuarios.FirstOrDefault(x => x.Email.ToLower() == email.ToLower());
        }
    }
}

