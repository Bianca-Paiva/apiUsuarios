using apiUsuarios.Data;
using apiUsuarios.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace apiUsuarios.Controllers
{

    // Controlador de API responsável pelas operações relacionadas a usuários.
    // Rota base: [controller] -> "Usuario"
    [ApiController]
    [Route("[controller]")]
    public class UsuarioController : ControllerBase
    {
        // Contexto do aplicativo para acesso ao banco de dados (injeção pelo construtor).
        private readonly AppDbContext _appDbContext;

        // Construtor que recebe uma instância de <see cref="AppDbContext"/> via injeção de dependência.
        // <param name="appDbContext">Contexto do banco de dados.</param>
        public UsuarioController(AppDbContext appDbContext)
        {
            // Atribui a instância injetada ao campo para uso nos métodos do controlador.
            _appDbContext = appDbContext;
        }

        // Retorna todos os usuários cadastrados.
        /// <returns>200 OK com a lista de usuários.</returns>
        [HttpGet("getAll")]
        public async Task<IActionResult> GetAllAsync()
        {
            // Recupera todos os registros da tabela Usuarios de forma assíncrona.
            List<Usuario> userList = await _appDbContext.Usuarios.ToListAsync();

            // Retorna a lista em um 200 OK.
            return Ok(userList);
        }


        // Cria um novo usuário a partir dos dados recebidos no corpo da requisição.

        // <param name="dadosUsuario">DTO com os dados do usuário a ser criado.</param>
        // <returns>200 OK quando criado com sucesso, 400 BadRequest em caso de erro.</returns>
        [HttpPost("create")]
        public async Task<IActionResult> CreateUserAsync([FromBody] CreateUserDTO dadosUsuario)
        {
            // Valida o modelo de entrada (atributos de validação em CreateUserDTO).
            if (!ModelState.IsValid)
            {
                // Retorna 400 quando algum campo obrigatório estiver inválido.
                return BadRequest("Dados inválidos");
            }

            // Mapeia o DTO para a entidade de domínio Usuario.
            Usuario usuarioSalvar = new Usuario
            {
                Nome = dadosUsuario.Nome,
                Email = dadosUsuario.Email,
                Senha = dadosUsuario.Senha,
                CPF = dadosUsuario.CPF,
            };

            // Marca a entidade para inserção no contexto.
            _appDbContext.Usuarios.Add(usuarioSalvar);

            // Persiste as alterações no banco de dados de forma assíncrona.
            int result = await _appDbContext.SaveChangesAsync();

            // Se houve alterações salvas (result > 0), considera-se sucesso.
            if (result > 0)
            {
                return Ok("Usuário criado com sucesso");
            }

            // Se não foram salvas alterações, retorna erro genérico de criação.
            return BadRequest("Erro ao criar o usuário");
        }

        // Tenta autenticar o usuário usando email e senha.

        // <param name="dadosLogin">DTO contendo Email e Senha.</param>

        // <returns>
        // 200 OK quando as credenciais estiverem corretas,
        // 404 NotFound quando o usuário não existir,
        // 401 Unauthorized quando a senha estiver incorreta,
        // 400 BadRequest quando o modelo for inválido.
        // </returns>
        [HttpPost("login")]
        public async Task<ActionResult> LoginAsync([FromBody] LoginDTO dadosLogin)
        {
            // Validação do DTO de login.
            if (!ModelState.IsValid)
            {
                return BadRequest("Dados de login inválidos");
            }

            // Procura o usuário pelo email (pode retornar null).
            Usuario? usuarioEncontrado = await _appDbContext.Usuarios.FirstOrDefaultAsync(usuario => usuario.Email == dadosLogin.Email);

            if (usuarioEncontrado == null)
            {
                // Se não encontrar, retorna 404 NotFound.
                return NotFound("Usuário não encontrado");
            }

            // Compara a senha enviada com a senha armazenada.
            // Observação: para produção, use hashing + salting em vez de comparação direta.
            if (usuarioEncontrado.Senha == dadosLogin.Senha)
            {
                return Ok("Login realizado");
            }

            // Se a senha estiver incorreta, retorna 401 Unauthorized.
            return Unauthorized("Senha incorreta");
        }
    }
}