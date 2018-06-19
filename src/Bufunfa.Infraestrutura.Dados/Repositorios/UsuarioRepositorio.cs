using JNogueira.Bufunfa.Dominio.Entidades;
using JNogueira.Bufunfa.Dominio.Interfaces.Dados;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace JNogueira.Bufunfa.Infraestrutura.Dados.Repositorios
{
    public class UsuarioRepositorio : IUsuarioRepositorio
    {
        private readonly EfDataContext _efContext;

        public UsuarioRepositorio(EfDataContext efContext)
        {
            _efContext = efContext;
        }

        public Usuario ObterPorEmailSenha(string email, string senha)
        {
            return _efContext
                .Usuarios
                .Where(x => x.Email == email && x.Senha == senha)
                .AsNoTracking()
                .FirstOrDefault();
        }
    }
}
