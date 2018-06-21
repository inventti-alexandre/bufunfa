using JNogueira.Bufunfa.Dominio.Entidades;
using JNogueira.Bufunfa.Dominio.Interfaces.Dados;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;

namespace JNogueira.Bufunfa.Infraestrutura.Dados.Repositorios
{
    public class UsuarioRepositorio : IUsuarioRepositorio
    {
        private readonly EfDataContext _efContext;

        public UsuarioRepositorio(EfDataContext efContext)
        {
            _efContext = efContext;
        }

        public async Task<Usuario> ObterPorEmailSenha(string email, string senha)
        {
            return await _efContext
                .Usuarios
                .Where(x => x.Email == email && x.Senha == senha)
                .AsNoTracking()
                .FirstOrDefaultAsync();
        }
    }
}
