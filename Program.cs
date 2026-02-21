using apiUsuarios.Data;
using Microsoft.EntityFrameworkCore;
using Scalar.AspNetCore;

namespace apiUsuarios
{
    public class Program
    {
        public static void Main(string[] args)
        {
            // Cria o "builder" da aplicação (configuração inicial do projeto, tipo ligar um pc)
            var builder = WebApplication.CreateBuilder(args);


            // =============================
            // REGISTRO DE SERVIÇOS
            // =============================

            // Habilita o uso de Controllers (necessário para usar rotas HTTP)
            builder.Services.AddControllers();


            // Habilita documentação OpenAPI (Swagger alternativo)
            builder.Services.AddOpenApi();


            // Configuração do Entity Framework Core para usar SQL Server
            // Aqui estamos dizendo:
            // - Use AppDbContext como contexto do banco
            // - Pegue a string de conexão do appsettings.json
           builder.Services.AddDbContext<AppDbContext>(options => options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));


            // Constrói a aplicação com tudo configurado acima
            var app = builder.Build();


            // =============================
            // CONFIGURAÇÃO DO PIPELINE HTTP
            // =============================

            // Se estiver em ambiente de desenvolvimento
            if (app.Environment.IsDevelopment())
            {
                // Mapeia documentação OpenAPI
                app.MapOpenApi();
                app.MapScalarApiReference();
            }

            // Força redirecionamento para HTTPS
            app.UseHttpsRedirection();

            // Habilita sistema de autorização
            // (obs: ainda não configurado a autenticação real)
            app.UseAuthorization();

            // Ativa os Controllers (sem isso as rotas não funcionam)
            app.MapControllers();

            // Inicia a aplicação
            app.Run();
        }
    }
}
