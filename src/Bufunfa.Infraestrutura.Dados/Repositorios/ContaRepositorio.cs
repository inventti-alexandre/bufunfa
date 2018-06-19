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

        public bool VerificarExistenciaPorNome(int idUsuario, string nome, int? idConta = null)
        {
            return idConta.HasValue
                ? _efContext.Contas.Any(x => x.IdUsuario == idUsuario && x.Nome.Equals(nome, System.StringComparison.InvariantCultureIgnoreCase) && x.Id != idConta)
                : _efContext.Contas.Any(x => x.IdUsuario == idUsuario && x.Nome.Equals(nome, System.StringComparison.InvariantCultureIgnoreCase));
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

        public void Deletar(Conta conta)
        {
            _efContext.Contas.Remove(conta);
        }
    }
}
