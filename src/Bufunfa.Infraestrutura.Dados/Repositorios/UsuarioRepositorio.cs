using Dapper;
using JNogueira.Bufunfa.Dominio.Comandos.Entrada;
using JNogueira.Bufunfa.Dominio.Entidades;
using JNogueira.Bufunfa.Dominio.Interfaces.Dados;
using System.Linq;

namespace JNogueira.Bufunfa.Infraestrutura.Dados.Repositorios
{
    public class UsuarioRepositorio : IUsuarioRepositorio
    {
        private readonly BufunfaDataContext _dbContext;

        public UsuarioRepositorio(BufunfaDataContext dbContext)
        {
            _dbContext = dbContext;
        }

        public void AlterarSenhaUsuario(AlterarSenhaUsuarioComando entrada)
        {
            throw new System.NotImplementedException();
        }

        public Usuario ObterPorEmailSenha(string email, string senha)
        {
            return _dbContext
                .Connection
                .Query<Usuario>("SELECT IdUsuario, Nome, Email, Ativo FROM usuario WHERE Email = @email AND Senha = @senha", new { email, senha })
                .FirstOrDefault();
        }
    }
}
