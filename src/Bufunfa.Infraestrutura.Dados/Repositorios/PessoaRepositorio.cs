using JNogueira.Bufunfa.Dominio.Comandos.Entrada;
using JNogueira.Bufunfa.Dominio.Comandos.Saida;
using JNogueira.Bufunfa.Dominio.Entidades;
using JNogueira.Bufunfa.Dominio.Interfaces.Dados;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using X.PagedList;

namespace JNogueira.Bufunfa.Infraestrutura.Dados.Repositorios
{
    public class PessoaRepositorio : IPessoaRepositorio
    {
        private readonly EfDataContext _efContext;

        public PessoaRepositorio(EfDataContext efContext)
        {
            _efContext = efContext;
        }

        public async Task<Pessoa> ObterPorId(int idPessoa, bool habilitarTracking = false)
        {
            var query = _efContext.Pessoas.AsQueryable();

            if (!habilitarTracking)
                query = query.AsNoTracking();

            return await query.FirstOrDefaultAsync(x => x.Id == idPessoa);
        }

        public async Task<ProcurarSaida> Procurar(ProcurarPessoaEntrada procurarEntrada)
        {
            var query = _efContext.Pessoas
                .AsNoTracking()
                .AsQueryable();

            if (!string.IsNullOrEmpty(procurarEntrada.Nome))
                query = query.Where(x => x.Nome.Contains(procurarEntrada.Nome));

            query = query.OrderByProperty(procurarEntrada.OrdenarPor, procurarEntrada.OrdenarSentido);

            if (procurarEntrada.Paginar())
            {
                var pagedList = await query.ToPagedListAsync(procurarEntrada.PaginaIndex.Value, procurarEntrada.PaginaTamanho.Value);

                return new ProcurarSaida(
                    pagedList.ToList().Select(x => new PessoaSaida(x)),
                    procurarEntrada.OrdenarPor,
                    procurarEntrada.OrdenarSentido,
                    pagedList.TotalItemCount,
                    pagedList.PageCount,
                    procurarEntrada.PaginaIndex,
                    procurarEntrada.PaginaTamanho);
            }
            else
            {
                var totalRegistros = await query.CountAsync();

                return new ProcurarSaida(
                    (await query.ToListAsync()).Select(x => new PessoaSaida(x)),
                    procurarEntrada.OrdenarPor,
                    procurarEntrada.OrdenarSentido,
                    totalRegistros);
            }
        }

        public async Task<bool> VerificarExistenciaPorNome(int idUsuario, string nome, int? idPessoa = null)
        {
            return idPessoa.HasValue
                ? await _efContext.Contas.AnyAsync(x => x.IdUsuario == idUsuario && x.Nome.Equals(nome, StringComparison.InvariantCultureIgnoreCase) && x.Id != idPessoa)
                : await _efContext.Contas.AnyAsync(x => x.IdUsuario == idUsuario && x.Nome.Equals(nome, StringComparison.InvariantCultureIgnoreCase));
        }

        public async Task<IEnumerable<Pessoa>> ObterPorUsuario(int idUsuario)
        {
            return await _efContext
                   .Pessoas
                   .AsNoTracking()
                   .Where(x => x.IdUsuario == idUsuario)
                   .ToListAsync();
        }

        public async Task Inserir(Pessoa pessoa)
        {
            await _efContext.AddAsync(pessoa);
        }

        public void Atualizar(Pessoa pessoa)
        {
            _efContext.Entry(pessoa).State = EntityState.Modified;
        }

        public void Deletar(Pessoa pessoa)
        {
            _efContext.Pessoas.Remove(pessoa);
        }
    }
}