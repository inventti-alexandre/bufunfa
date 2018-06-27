using JNogueira.Bufunfa.Dominio.Comandos.Entrada;
using JNogueira.Bufunfa.Dominio.Entidades;
using JNogueira.Bufunfa.Dominio.Interfaces.Dados;
using Microsoft.EntityFrameworkCore;
using PagedList;
using System;
using System.Collections.Generic;
using System.Linq;

namespace JNogueira.Bufunfa.Infraestrutura.Dados.Repositorios
{
    public class PessoaRepositorio : IPessoaRepositorio
    {
        private readonly EfDataContext _efContext;

        public PessoaRepositorio(EfDataContext efContext)
        {
            _efContext = efContext;
        }

        public Pessoa ObterPorId(int idPessoa, bool habilitarTracking = false)
        {
            var query = _efContext.Pessoas.AsQueryable();

            if (!habilitarTracking)
                query = query.AsNoTracking();

            return query.FirstOrDefault(x => x.Id == idPessoa);
        }

        public void Inserir(Pessoa pessoa)
        {
            _efContext.Add(pessoa);
        }

        public bool VerificarExistenciaPorNome(int idUsuario, string nome, int? idPessoa = null)
        {
            return idPessoa.HasValue
                ? _efContext.Contas.Any(x => x.IdUsuario == idUsuario && x.Nome.Equals(nome, StringComparison.InvariantCultureIgnoreCase) && x.Id != idPessoa)
                : _efContext.Contas.Any(x => x.IdUsuario == idUsuario && x.Nome.Equals(nome, StringComparison.InvariantCultureIgnoreCase));
        }

        public IEnumerable<Pessoa> ObterPorUsuario(int idUsuario)
        {
            return _efContext
                   .Pessoas
                   .AsNoTracking()
                   .Where(x => x.IdUsuario == idUsuario)
                   .AsEnumerable();
        }

        public void Atualizar(Pessoa pessoa)
        {
            _efContext.Entry(pessoa).State = EntityState.Modified;
        }

        public void Deletar(Pessoa pessoa)
        {
            _efContext.Pessoas.Remove(pessoa);
        }

        public IEnumerable<Pessoa> Procurar(ProcurarPessoaEntrada buscaEntrada)
        {
            var query = _efContext.Pessoas
                .AsNoTracking()
                .AsQueryable();

            if (!string.IsNullOrEmpty(buscaEntrada.Nome))
                query = query.Where(x => x.Nome.Contains(buscaEntrada.Nome));

            query = query.OrderByExtended(buscaEntrada.OrdenarPor, buscaEntrada.OrdenarSentido);

            buscaEntrada.TotalRegistros = query.Count();

            return buscaEntrada.Paginar()
                ? query.ToPagedList(buscaEntrada.PaginaIndex, buscaEntrada.PaginaTamanho).ToList()
                : query.ToList();
        }
    }
}