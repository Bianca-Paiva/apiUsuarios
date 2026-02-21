namespace apiUsuarios.Models
{
    // DTO usado para receber dados no cadastro
    // Não representa tabela, só transporte de dados
    public class CreateUserDTO
    {
        public string Nome { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Senha { get; set; } = string.Empty;
        public string CPF { get; set; } = string.Empty;
    }
}
