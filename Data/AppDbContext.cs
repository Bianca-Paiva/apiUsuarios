using apiUsuarios.Models;
using Microsoft.EntityFrameworkCore;

namespace apiUsuarios.Data
{
    public class AppDbContext : DbContext
    {
        // Construtor para injeção de dependência
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }

        // Define um DbSet para a entidade Usuario, que representa a tabela de usuários no banco de dados
        public DbSet<Usuario> Usuarios { get; set; }


    }
}
