using JNogueira.Bufunfa.Dominio;
using JNogueira.Bufunfa.Dominio.Comandos.Entrada;
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
    public class AgendamentoRepositorio : IAgendamentoRepositorio
    {
        private readonly EfDataContext _efContext;

        public AgendamentoRepositorio(EfDataContext efContext)
        {
            _efContext = efContext;
        }

        public async Task<Agendamento> ObterPorId(int idAgendamento, bool habilitarTracking = false)
        {
            var query = _efContext.Agendamentos
                .Include(x => x.Conta)
                .Include(x => x.CartaoCredito)
                .Include(x => x.Categoria)
                    .ThenInclude(x => x.CategoriaPai)
                    .ThenInclude(x => x.CategoriasFilha)
                .Include(x => x.Pessoa)
                .Include(x => x.Parcelas)
                .AsQueryable();

            if (!habilitarTracking)
                query = query.AsNoTracking();

            return await query.FirstOrDefaultAsync(x => x.Id == idAgendamento);
        }

        public async Task<ProcurarSaida> Procurar(ProcurarAgendamentoEntrada procurarEntrada)
        {
            var query = _efContext.Agendamentos
                .Include(x => x.Conta)
                .Include(x => x.CartaoCredito)
                .Include(x => x.Categoria)
                    .ThenInclude(x => x.CategoriaPai)
                    .ThenInclude(x => x.CategoriasFilha)
                .Include(x => x.Pessoa)
                .Include(x => x.Parcelas)
                .AsNoTracking()
                .AsQueryable();

            if (procurarEntrada.IdCategoria.HasValue)
                query = query.Where(x => x.IdCategoria == procurarEntrada.IdCategoria);

            if (procurarEntrada.IdConta.HasValue)
                query = query.Where(x => x.IdConta == procurarEntrada.IdConta);

            if (procurarEntrada.IdCartaoCredito.HasValue)
                query = query.Where(x => x.IdCartaoCredito == procurarEntrada.IdCartaoCredito);

            if (procurarEntrada.IdPessoa.HasValue)
                query = query.Where(x => x.IdPessoa == procurarEntrada.IdPessoa);

            if (procurarEntrada.DataInicioParcela.HasValue && procurarEntrada.DataFimParcela.HasValue)
                query = query.Where(x => x.Parcelas.Any(y => y.Data.Date >= procurarEntrada.DataInicioParcela.Value.Date && y.Data.Date <= procurarEntrada.DataFimParcela.Value.Date));

            if (procurarEntrada.Concluido.HasValue)
            {
                query = procurarEntrada.Concluido.Value
                    ? query.Where(x => x.Parcelas.Count(y => y.Status == StatusParcela.Aberta) == x.Parcelas.Count(y => y.Status == StatusParcela.Fechada))
                    : query.Where(x => x.Parcelas.Count(y => y.Status == StatusParcela.Aberta) != x.Parcelas.Count(y => y.Status == StatusParcela.Fechada));
            }

            query = query.OrderByProperty(procurarEntrada.OrdenarPor, procurarEntrada.OrdenarSentido);

            if (procurarEntrada.Paginar())
            {
                var pagedList = await query.ToPagedListAsync(procurarEntrada.PaginaIndex.Value, procurarEntrada.PaginaTamanho.Value);

                return new ProcurarSaida(
                    pagedList.ToList().Select(x => new AgendamentoSaida(x)),
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
                    (await query.ToListAsync()).Select(x => new AgendamentoSaida(x)),
                    procurarEntrada.OrdenarPor,
                    procurarEntrada.OrdenarSentido,
                    totalRegistros);
            }
        }

        public async Task Inserir(Agendamento agendamento)
        {
            await _efContext.AddAsync(agendamento);
        }

        public void Atualizar(Agendamento agendamento)
        {
            _efContext.Entry(agendamento).State = EntityState.Modified;
        }

        public void Deletar(Agendamento agendamento)
        {
            _efContext.Agendamentos.Remove(agendamento);
        }
    }
}