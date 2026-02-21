using apiUsuarios.Data;
using apiUsuarios.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace apiUsuarios.Controllers
{

    // Define como controller de API (tradução humana: "Isso aqui é uma API, trate como API”)
    [ApiController]

    // Define a rota base como "Usuario"
    // Exemplo: /Usuario/create
    [Route("[controller]")]

    // Construtor diz: “Ei sistema, me entrega a conexão com o banco.”. Isso chama injeção de dependência.
    // Resumindo: O controller não cria o banco. Ele recebe pronto.
    public class UsuarioController : ControllerBase
    {
        // Campo privado para acessar o banco
        private readonly AppDbContext _appDbContext;

        // Construtor recebe o contexto por injeção de dependência
        public UsuarioController(AppDbContext appDbContext)
        {
            // Atribui a instância injetada ao campo para uso nos métodos do controlador.
            _appDbContext = appDbContext;
        }


        // ===================================
        // GET (cria a rota): /Usuario/getAll
        // ===================================
        // Fluxo:
        // 1. Pede a lista
        // 2. Banco responde
        // 3. API devolve
        [HttpGet("getAll")]
        public async Task<IActionResult> GetAllAsync()
        {
            // Busca todos os usuários no banco (igual a um "SELECT * FROM Usuarios")
            List<Usuario> userList = await _appDbContext.Usuarios.ToListAsync();

            // Retorna 200 OK com a lista
            return Ok(userList);
        }


        // ==========================================
        // POST (fluxo de cadastro): /Usuario/create
        // ==========================================

        // Fluxo:
        // 1. Recebe JSON
        // 2. Converte pra CreateUserDTO
        // 3. Cria um Usuario
        // 4. Adiciona no DbContext
        // 5. Salva no banco
        // 6. Retorna sucesso
        [HttpPost("create")]

        // [FromBody] CreateUserDTO dadosUsuario: pega os dados do usuário no json e converte para o c#.
        public async Task<IActionResult> CreateUserAsync([FromBody] CreateUserDTO dadosUsuario)
        {
            // Verifica se os dados recebidos são válidos
            if (!ModelState.IsValid)
            {
                // Retorna 400 quando algum campo obrigatório estiver inválido.
                return BadRequest("Dados inválidos");
            }

            // Converte o DTO em entidade Usuario, ou seja, copia os dados convertidos no "[FromBody] CreateUserDTO dadosUsuario" e cola no banco de dados.
            Usuario usuarioSalvar = new Usuario
            {
                Nome = dadosUsuario.Nome,
                Email = dadosUsuario.Email,
                Senha = dadosUsuario.Senha,
                CPF = dadosUsuario.CPF,
            };

            // Marca para inserção no banco
            // Add = prepara
            _appDbContext.Usuarios.Add(usuarioSalvar);

            // Salva no banco
            // SaveChanges = grava no banco (sem ele, nada acontece.)
            int result = await _appDbContext.SaveChangesAsync();

            // Se salvou corretamente:
            if (result > 0)
            {
                return Ok("Usuário criado com sucesso");
            }

            // Se não salvou:
            return BadRequest("Erro ao criar o usuário");
        }


        // =============================
        // POST: /Usuario/login
        // =============================
        // Tenta autenticar o usuário usando email e senha.
        [HttpPost("login")]

        // [FromBody] CreateUserDTO dadosUsuario: pega os dados do usuário no json e converte para o c#.
        public async Task<ActionResult> LoginAsync([FromBody] LoginDTO dadosLogin)
        {
            // Validação do DTO de login.
            if (!ModelState.IsValid)
            {
                return BadRequest("Dados de login inválidos");
            }

            // Procura o usuário pelo email (pode retornar null).
            // Isso vira um "SELECT TOP 1 * FROM Usuarios WHERE Email = '...'" no banco.
            Usuario? usuarioEncontrado = await _appDbContext.Usuarios.FirstOrDefaultAsync(usuario => usuario.Email == dadosLogin.Email);

            // Se não encontrar → NotFound (404)
            if (usuarioEncontrado == null)
            {
                return NotFound("Usuário não encontrado");
            }

            // Se achar → compara senha a senha enviada com a senha armazenada:
            if (usuarioEncontrado.Senha == dadosLogin.Senha)
            {
                // Se a senha enviada e a senha armazenada no banco de dados forem iguais, exibe a mensagem de login realizado.
                return Ok("Login realizado");
            }

            // Se a senha estiver diferente da armazenada no banco de dados, retorna 401 Unauthorized.
            return Unauthorized("Senha incorreta");
        }
    }
}