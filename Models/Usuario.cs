using System.ComponentModel.DataAnnotations;

namespace apiUsuarios.Models
{
    public class Usuario
    {
        [Key]
        public int Id { get; set; }

        // Validações de dados usando Data Annotations
        [Required(ErrorMessage = "Nome é um campo obrigatório")]
        [StringLength(100, MinimumLength = 3, ErrorMessage = "O nome deve possuir de 3 a 100 caractéres")]
        public string Nome { get; set; } = string.Empty;


        // Validações de email usando Data Annotations
        [Required(ErrorMessage = "Email é um campo obrigatório")]
        [EmailAddress(ErrorMessage = "Email inválido")]
        public string Email { get; set; } = string.Empty;


        // Validações de senha usando Data Annotations
        [Required(ErrorMessage = "Senha é um campo obrigatório")]
        [StringLength(100, MinimumLength = 6, ErrorMessage = "A senha deve possuir de 6 a 100 caractéres")]
        public string Senha { get; set; } = string.Empty;


        // Validações de CPF usando Data Annotations
        [Required(ErrorMessage = "CPF é um campo obrigatório")]
        [StringLength(100, MinimumLength = 11, ErrorMessage = "O CPF deve conter exatamente 11 caractéres")]
        public string CPF { get; set; } = string.Empty;

        // A data de criação do usuário é definida automaticamente para a data atual quando um novo usuário é criado
        public DateTime DataCriacao { get; set; } = DateTime.Now;        
    }
}
