using JNogueira.Bufunfa.Dominio.Entidades;
using JNogueira.Bufunfa.Dominio.Interfaces.Dados;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using X.PagedList;

namespace JNogueira.Bufunfa.Infraestrutura.Dados.Repositorios
{
    public class ParcelaRepositorio : IParcelaRepositorio
    {
        private readonly EfDataContext _efContext;

        public ParcelaRepositorio(EfDataContext efContext)
        {
            _efContext = efContext;
        }

        public async Task<Parcela> ObterPorId(int idParcela, bool habilitarTracking = false)
        {
            var query = _efContext.Parcelas
                .Include(x => x.Agendamento)
                .AsQueryable();

            if (!habilitarTracking)
                query = query.AsNoTracking();

            return await query.FirstOrDefaultAsync(x => x.Id == idParcela);
        }

        public async Task<IEnumerable<Parcela>> ObterPorUsuario(int idUsuario)
        {
            return await _efContext
                   .Parcelas
                   .Include(x => x.Agendamento)
                   .OrderBy(x => x.Data)
                   .ToListAsync();
        }

        public async Task Inserir(Parcela parcela)
        {
            await _efContext.AddAsync(parcela);
        }

        public async Task Inserir(IEnumerable<Parcela> parcelas)
        {
            await _efContext.AddRangeAsync(parcelas);
        }

        public void Atualizar(Parcela parcela)
        {
            _efContext.Entry(parcela).State = EntityState.Modified;
        }

        public void Deletar(Parcela parcela)
        {
            _efContext.Parcelas.Remove(parcela);
        }

        public void Deletar(IEnumerable<Parcela> parcelas)
        {
            _efContext.Parcelas.RemoveRange(parcelas);
        }

        public async Task<bool> VerificarExistenciaPorId(int idUsuario, int idParcela)
        {
            return await _efContext.Parcelas.AnyAsync(x => x.Id == idParcela);
        }
    }
}