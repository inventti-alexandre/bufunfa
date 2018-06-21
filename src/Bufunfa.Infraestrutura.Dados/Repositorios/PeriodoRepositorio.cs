using JNogueira.Bufunfa.Dominio.Entidades;
using JNogueira.Bufunfa.Dominio.Interfaces.Dados;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace JNogueira.Bufunfa.Infraestrutura.Dados.Repositorios
{
    public class PeriodoRepositorio : IPeriodoRepositorio
    {
        private readonly EfDataContext _efContext;

        public PeriodoRepositorio(EfDataContext efContext)
        {
            _efContext = efContext;
        }

        public async Task<Periodo> ObterPorId(int idPeriodo, bool habilitarTracking = false)
        {
            var query = _efContext.Periodos.AsQueryable();

            if (!habilitarTracking)
                query = query.AsNoTracking();

            return await query.FirstOrDefaultAsync(x => x.Id == idPeriodo);
        }

        public async Task<IEnumerable<Periodo>> ObterPorUsuario(int idUsuario)
        {
            return await _efContext
                   .Periodos
                   .AsNoTracking()
                   .Where(x => x.IdUsuario == idUsuario)
                   .ToListAsync();
        }

        public async Task Inserir(Periodo periodo)
        {
           await _efContext.AddAsync(periodo);
        }

        public void Atualizar(Periodo periodo)
        {
            _efContext.Entry(periodo).State = EntityState.Modified;
        }

        public void Deletar(Periodo periodo)
        {
            _efContext.Periodos.Remove(periodo);
        }

        public async Task<bool> VerificarExistenciaPorDataInicioFim(int idUsuario, DateTime dataInicio, DateTime dataFim, int? idPeriodo = null)
        {
            dataInicio = dataInicio.Date;
            dataFim = dataFim.Date.AddHours(23).AddMinutes(59).AddSeconds(59);

            return idPeriodo.HasValue
                ? await _efContext.Periodos.AnyAsync(x => x.IdUsuario == idUsuario && (x.DataInicio < dataFim && dataInicio < x.DataFim) && x.Id != idPeriodo)
                : await _efContext.Periodos.AnyAsync(x => x.IdUsuario == idUsuario && (x.DataInicio < dataFim && dataInicio < x.DataFim));
        }
    }
}