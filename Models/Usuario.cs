using System.ComponentModel.DataAnnotations;

namespace apiUsuarios.Models
{
    // Model = dados que vão pro banco (vira tabela)
    // Estrutura da tabela no banco. Cada propriedade vai virar uma coluna.
    public class Usuario
    {
        // Define como chave primária
        [Key]
        public int Id { get; set; }

        // Campo obrigatório
        // Mínimo 3 e máximo 100 caracteres
        [Required(ErrorMessage = "Nome é um campo obrigatório")]
        [StringLength(100, MinimumLength = 3, ErrorMessage = "O nome deve possuir de 3 a 100 caracteres")]
        public string Nome { get; set; } = string.Empty;


        // Validação de email
        [Required(ErrorMessage = "Email é obrigatório")]
        [EmailAddress(ErrorMessage = "Email inválido")]
        public string Email { get; set; } = string.Empty;


        // Validação de senha
        [Required(ErrorMessage = "Senha é obrigatória")]
        [StringLength(100, MinimumLength = 6,
            ErrorMessage = "A senha deve possuir de 6 a 100 caracteres")]
        public string Senha { get; set; } = string.Empty;


        // Validação de CPF
        [Required(ErrorMessage = "CPF é obrigatório")]
        [StringLength(11, MinimumLength = 11,
            ErrorMessage = "O CPF deve conter exatamente 11 caracteres")]
        public string CPF { get; set; } = string.Empty;


        // Data e horário criados automaticamente quando o usuário é instanciado
        public DateTime DataCriacao { get; set; } = DateTime.Now;
    }
}
