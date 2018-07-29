using JNogueira.Bufunfa.Dominio.Entidades;
using JNogueira.Bufunfa.Dominio.Interfaces.Dados;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;

namespace JNogueira.Bufunfa.Infraestrutura.Dados.Repositorios
{
    public class AnexoRepositorio : IAnexoRepositorio
    {
        private readonly EfDataContext _efContext;

        public AnexoRepositorio(EfDataContext efContext)
        {
            _efContext = efContext;
        }

        public async Task<Anexo> ObterPorId(int idAnexo, bool habilitarTracking = false)
        {
            var query = _efContext.Anexos.AsQueryable();

            if (!habilitarTracking)
                query = query.AsNoTracking();

            return await query.FirstOrDefaultAsync(x => x.Id == idAnexo);
        }

        public async Task Inserir(Anexo anexo)
        {
            await _efContext.AddAsync(anexo);
        }

        public void Deletar(Anexo anexo)
        {
            _efContext.Anexos.Remove(anexo);
        }
    }
}