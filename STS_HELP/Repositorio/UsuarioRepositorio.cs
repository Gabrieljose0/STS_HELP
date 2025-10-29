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

            // 2. Atualiza os dados do perfil local (Nome, Email, Tipo)
            usuarioDB.Nome = usuarios.Nome;
            usuarioDB.Email = usuarios.Email;
            usuarioDB.TipoUsuario = usuarios.TipoUsuario;
            _bancoContext.Usuarios.Update(usuarioDB);

            // 3. --- LÓGICA DA NOVA SENHA (VIA HTTP) ---
            if (!string.IsNullOrEmpty(usuarios.SenhaParaCadastro))
            {
                if (usuarioDB.AuthId == null)
                {
                    throw new Exception("Usuário não vinculado à autenticação.");
                }

                // Pega a URL base e a ServiceKey da configuração (precisamos delas de novo)
                // Precisamos injetar IConfiguration também no repositório
                // (Veja Passo 3 abaixo)
                var supabaseUrl = _configuration["Supabase:Url"];
                var supabaseServiceKey = _configuration["Supabase:ServiceKey"];
                var userId = usuarioDB.AuthId.ToString();

                // Monta a URL da API Admin
                // Ex: https://kxg...co/auth/v1/admin/users/UUID_DO_USUARIO
                var requestUrl = $"{supabaseUrl}/auth/v1/admin/users/{userId}";

                // Cria o corpo da requisição (payload JSON)
                var payload = new { password = usuarios.SenhaParaCadastro };
                var jsonPayload = JsonSerializer.Serialize(payload); // Ou JsonConvert.SerializeObject(payload);
                var httpContent = new StringContent(jsonPayload, Encoding.UTF8, "application/json");

                // Cria o cliente HTTP
                var httpClient = _httpClientFactory.CreateClient();

                // Cria a requisição PUT (ou PATCH, dependendo da API)
                // A documentação do Supabase indica PUT para /admin/users/{user_id}
                var request = new HttpRequestMessage(HttpMethod.Put, requestUrl);

                // Adiciona os Headers OBRIGATÓRIOS de Admin
                request.Headers.Add("apikey", supabaseServiceKey);
                request.Headers.Add("Authorization", $"Bearer {supabaseServiceKey}");
                request.Content = httpContent;

                // Envia a requisição
                var response = await httpClient.SendAsync(request);

                // Verifica se a API retornou sucesso (ex: 200 OK)
                if (!response.IsSuccessStatusCode)
                {
                    // Se falhou, lê a mensagem de erro da API e lança uma exceção
                    var errorContent = await response.Content.ReadAsStringAsync();
                    throw new Exception($"Erro ao atualizar senha no Supabase: {response.StatusCode} - {errorContent}");
                }
                // Se chegou aqui, a senha foi atualizada com sucesso no Supabase Auth
            }

            // 4. Salva as mudanças do perfil (Nome/Email/Tipo) no seu banco
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

            // Altere o status do usuário encontrado

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

