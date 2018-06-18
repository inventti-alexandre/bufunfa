using JNogueira.Bufunfa.Dominio.Entidades;
using JNogueira.Bufunfa.Dominio.Interfaces.Dados;
using Microsoft.EntityFrameworkCore;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace JNogueira.Bufunfa.Infraestrutura.Dados.Repositorios
{
    public class PeriodoRepositorio : IPeriodoRepositorio
    {
        private readonly EfDataContext _efContext;

        public PeriodoRepositorio(EfDataContext efContext)
        {
            _efContext = efContext;
        }

        public Periodo ObterPorId(int idPeriodo, bool habilitarTracking = false)
        {
            var query = _efContext.Periodos.AsQueryable();

            if (!habilitarTracking)
                query = query.AsNoTracking();

            return query.FirstOrDefault(x => x.Id == idPeriodo);
        }

        public void Inserir(Periodo periodo)
        {
            _efContext.Add(periodo);
        }

        public bool VerificarExistenciaPorDataInicioFim(int idUsuario, DateTime dataInicio, DateTime dataFim, int? idPeriodo = null)
        {
            dataInicio = dataInicio.Date;
            dataFim = dataFim.Date.AddHours(23).AddMinutes(59).AddSeconds(59);

            return idPeriodo.HasValue
                ? _efContext.Periodos.Any(x => x.IdUsuario == idUsuario && (x.DataInicio < dataFim && dataInicio < x.DataFim) && x.Id != idPeriodo)
                : _efContext.Periodos.Any(x => x.IdUsuario == idUsuario && (x.DataInicio < dataFim && dataInicio < x.DataFim));
        }

        public IEnumerable<Periodo> ObterPorUsuario(int idUsuario)
        {
            return _efContext
                   .Periodos
                   .AsNoTracking()
                   .Where(x => x.IdUsuario == idUsuario)
                   .AsEnumerable();
        }

        public void Atualizar(Periodo periodo)
        {
            _efContext.Entry(periodo).State = EntityState.Modified;
        }

        public void Deletar(int idPeriodo)
        {
            _efContext.Database.ExecuteSqlCommand(
                "DELETE FROM periodo WHERE IdPeriodo = @p0",
                new MySqlParameter { ParameterName = "p0", DbType = DbType.Int32, Value = idPeriodo });
        }
    }
}