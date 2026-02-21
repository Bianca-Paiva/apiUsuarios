using apiUsuarios.Models;
using Microsoft.EntityFrameworkCore;

namespace apiUsuarios.Data
{
    // Classe que representa a conexão com o banco de dados
    public class AppDbContext : DbContext
    {
        // Construtor que recebe as configurações do banco
        // Essa configuração vem do Program.cs
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }

        // Basicamente está dizendo: "Crie uma tabela chamada Usuarios baseada na classe Usuario"
        // DbSet representa uma TABELA no banco.
        public DbSet<Usuario> Usuarios { get; set; }


    }
}
