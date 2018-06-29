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
    public class CategoriaRepositorio : ICategoriaRepositorio
    {
        private readonly EfDataContext _efContext;

        public CategoriaRepositorio(EfDataContext efContext)
        {
            _efContext = efContext;
        }

        public async Task<Categoria> ObterPorId(int idCategoria, bool habilitarTracking = false)
        {
            var query = _efContext.Categorias
                    .Include(x => x.CategoriaPai)
                    .Include(x => x.CategoriasFilha)
                    .AsQueryable();

            if (!habilitarTracking)
                query = query.AsNoTracking();

            return await query.FirstOrDefaultAsync(x => x.Id == idCategoria);
        }

        //public async Task<ProcurarSaida> Procurar(ProcurarCategoriaEntrada procurarEntrada)
        //{
        //    var query = _efContext.Categorias
        //        .AsNoTracking()
        //        .AsQueryable();

        //    if (!string.IsNullOrEmpty(procurarEntrada.Nome))
        //        query = query.Where(x => x.Nome.Contains(procurarEntrada.Nome));

        //    query = query.OrderByProperty(procurarEntrada.OrdenarPor, procurarEntrada.OrdenarSentido);

        //    if (procurarEntrada.Paginar())
        //    {
        //        var pagedList = await query.ToPagedListAsync(procurarEntrada.PaginaIndex.Value, procurarEntrada.PaginaTamanho.Value);

        //        return new ProcurarSaida(
        //            pagedList.ToList().Select(x => new CategoriaSaida(x)),
        //            procurarEntrada.OrdenarPor,
        //            procurarEntrada.OrdenarSentido,
        //            pagedList.TotalItemCount,
        //            pagedList.PageCount,
        //            procurarEntrada.PaginaIndex,
        //            procurarEntrada.PaginaTamanho);
        //    }
        //    else
        //    {
        //        var totalRegistros = await query.CountAsync();

        //        return new ProcurarSaida(
        //            (await query.ToListAsync()).Select(x => new CategoriaSaida(x)),
        //            procurarEntrada.OrdenarPor,
        //            procurarEntrada.OrdenarSentido,
        //            totalRegistros);
        //    }
        //}

        //public async Task<bool> VerificarExistenciaPorNome(int idUsuario, string nome, int? idCategoria = null)
        //{
        //    return idCategoria.HasValue
        //        ? await _efContext.Contas.AnyAsync(x => x.IdUsuario == idUsuario && x.Nome.Equals(nome, StringComparison.InvariantCultureIgnoreCase) && x.Id != idCategoria)
        //        : await _efContext.Contas.AnyAsync(x => x.IdUsuario == idUsuario && x.Nome.Equals(nome, StringComparison.InvariantCultureIgnoreCase));
        //}

        public async Task<IEnumerable<Categoria>> ObterPorUsuario(int idUsuario)
        {
            return await _efContext
                   .Categorias
                   .AsNoTracking()
                   .Where(x => x.IdUsuario == idUsuario)
                   .ToListAsync();
        }

        public async Task Inserir(Categoria pessoa)
        {
            await _efContext.AddAsync(pessoa);
        }

        public void Atualizar(Categoria pessoa)
        {
            _efContext.Entry(pessoa).State = EntityState.Modified;
        }

        public void Deletar(Categoria pessoa)
        {
            _efContext.Categorias.Remove(pessoa);
        }
    }
}