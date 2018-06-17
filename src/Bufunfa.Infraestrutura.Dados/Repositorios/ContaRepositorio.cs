using JNogueira.Bufunfa.Dominio.Entidades;
using JNogueira.Bufunfa.Dominio.Interfaces.Dados;
using Microsoft.EntityFrameworkCore;
using MySql.Data.MySqlClient;
using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace JNogueira.Bufunfa.Infraestrutura.Dados.Repositorios
{
    public class ContaRepositorio : IContaRepositorio
    {
        private readonly EfDataContext _efContext;

        public ContaRepositorio(EfDataContext efContext)
        {
            _efContext = efContext;
        }

        public Conta ObterPorId(int idConta, bool habilitarTracking = false)
        {
            var query = _efContext.Contas.AsQueryable();

            if (!habilitarTracking)
                query = query.AsNoTracking();

            return query.FirstOrDefault(x => x.Id == idConta);
        }

        public void Inserir(Conta conta)
        {
            _efContext.Add(conta);
        }

        public bool VerificarExistenciaContaPorNome(int idUsuario, string nome)
        {
            return _efContext
                   .Contas
                   .Any(x => x.Nome.IndexOf(nome, System.StringComparison.InvariantCultureIgnoreCase) >= 0);
        }

        public IEnumerable<Conta> ObterPorUsuario(int idUsuario)
        {
            return _efContext
                   .Contas
                   .AsNoTracking()
                   .Where(x => x.IdUsuario == idUsuario)
                   .AsEnumerable();
        }

        public void Atualizar(Conta conta)
        {
            _efContext.Entry(conta).State = EntityState.Modified;
        }

        public void Deletar(int idConta)
        {
            _efContext.Database.ExecuteSqlCommand(
                "DELETE FROM conta WHERE IdConta = @p0",
                new MySqlParameter { ParameterName = "p0", DbType = DbType.Int32, Value = idConta });
        }
    }
}
