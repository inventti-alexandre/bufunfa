﻿using JNogueira.Bufunfa.Dominio.Comandos.Entrada;
using JNogueira.Bufunfa.Dominio.Comandos.Saida;
using JNogueira.Bufunfa.Dominio.Entidades;
using JNogueira.Bufunfa.Dominio.Interfaces.Dados;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using X.PagedList;

namespace JNogueira.Bufunfa.Infraestrutura.Dados.Repositorios
{
    public class LancamentoRepositorio : ILancamentoRepositorio
    {
        private readonly EfDataContext _efContext;

        public LancamentoRepositorio(EfDataContext efContext)
        {
            _efContext = efContext;
        }

        public async Task<Lancamento> ObterPorId(int idLancamento, bool habilitarTracking = false)
        {
            var query = _efContext.Lancamentos
                .Include(x => x.Conta)
                .Include(x => x.Categoria)
                    .ThenInclude(x => x.CategoriaPai)
                .Include(x => x.Pessoa)
                .Include(x => x.Anexos)
                .AsQueryable();

            if (!habilitarTracking)
                query = query.AsNoTracking();

            return await query.FirstOrDefaultAsync(x => x.Id == idLancamento);
        }

        public async Task<ProcurarSaida> Procurar(ProcurarLancamentoEntrada procurarEntrada)
        {
            var query = _efContext.Lancamentos
                .Include(x => x.Conta)
                .Include(x => x.Categoria)
                    .ThenInclude(x => x.CategoriaPai)
                .Include(x => x.Pessoa)
                .Include(x => x.Anexos)
                .AsNoTracking()
                .AsQueryable();

            if (procurarEntrada.IdConta.HasValue)
                query = query.Where(x => x.IdConta == procurarEntrada.IdConta.Value);

            if (procurarEntrada.IdCategoria.HasValue)
                query = query.Where(x => x.IdCategoria == procurarEntrada.IdCategoria.Value);

            if (procurarEntrada.IdPessoa.HasValue)
                query = query.Where(x => x.IdCategoria == procurarEntrada.IdPessoa.Value);

            if (procurarEntrada.DataInicio.HasValue && procurarEntrada.DataFim.HasValue)
                query = query.Where(x => x.Data.Date >= procurarEntrada.DataInicio.Value.Date && x.Data.Date <= procurarEntrada.DataFim.Value.Date);

            query = query.OrderByProperty(procurarEntrada.OrdenarPor, procurarEntrada.OrdenarSentido);

            if (procurarEntrada.Paginar())
            {
                var pagedList = await query.ToPagedListAsync(procurarEntrada.PaginaIndex.Value, procurarEntrada.PaginaTamanho.Value);

                return new ProcurarSaida(
                    pagedList.ToList().Select(x => new LancamentoSaida(x)),
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
                    (await query.ToListAsync()).Select(x => new LancamentoSaida(x)),
                    procurarEntrada.OrdenarPor,
                    procurarEntrada.OrdenarSentido,
                    totalRegistros);
            }
        }

        public async Task<IEnumerable<Lancamento>> ObterPorUsuario(int idUsuario)
        {
            return await _efContext
                   .Lancamentos
                   .Include(x => x.Conta)
                   .Include(x => x.Categoria)
                        .ThenInclude(x => x.CategoriaPai)
                   .Include(x => x.Pessoa)
                   .Include(x => x.Anexos)
                   .AsNoTracking()
                   .Where(x => x.IdUsuario == idUsuario)
                   .OrderBy(x => x.Data)
                   .ToListAsync();
        }

        public async Task Inserir(Lancamento lancamento)
        {
            await _efContext.AddAsync(lancamento);
        }

        public void Atualizar(Lancamento lancamento)
        {
            _efContext.Entry(lancamento).State = EntityState.Modified;
        }

        public void Deletar(Lancamento lancamento)
        {
            _efContext.Lancamentos.Remove(lancamento);
        }
    }
}