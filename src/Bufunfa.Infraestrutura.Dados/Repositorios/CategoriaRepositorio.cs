﻿using JNogueira.Bufunfa.Dominio.Comandos.Entrada;
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
                        .ThenInclude(y => y.CategoriasFilha)
                    .AsQueryable();

            if (!habilitarTracking)
                query = query.AsNoTracking();

            return await query.FirstOrDefaultAsync(x => x.Id == idCategoria);
        }

        public async Task<ProcurarSaida> Procurar(ProcurarCategoriaEntrada procurarEntrada)
        {
            var query = _efContext.Categorias
                .Include(x => x.CategoriaPai)
                .AsNoTracking()
                .AsQueryable();

            if (!string.IsNullOrEmpty(procurarEntrada.Nome))
                query = query.Where(x => x.Nome.Contains(procurarEntrada.Nome));

            if (!string.IsNullOrEmpty(procurarEntrada.Tipo))
                query = query.Where(x => x.Tipo.Equals(procurarEntrada.Tipo, StringComparison.InvariantCultureIgnoreCase));

            if (procurarEntrada.IdCategoriaPai.HasValue)
                query = query.Where(x => x.IdCategoriaPai.HasValue && x.IdCategoriaPai.Value == procurarEntrada.IdCategoriaPai.Value);

            query = query.OrderByProperty(procurarEntrada.OrdenarPor, procurarEntrada.OrdenarSentido);

            if (procurarEntrada.Paginar())
            {
                var pagedList = await query.ToPagedListAsync(procurarEntrada.PaginaIndex.Value, procurarEntrada.PaginaTamanho.Value);

                return new ProcurarSaida(
                    pagedList.ToList().Select(x => new CategoriaSaida(x)),
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
                    (await query.ToListAsync()).Select(x => new CategoriaSaida(x)),
                    procurarEntrada.OrdenarPor,
                    procurarEntrada.OrdenarSentido,
                    totalRegistros);
            }
        }

        public async Task<bool> VerificarExistenciaPorNomeTipo(int idUsuario, string nome, string tipo, int? idCategoria = null)
        {
            return idCategoria.HasValue
                ? await _efContext.Categorias.AnyAsync(x => x.IdUsuario == idUsuario && x.Nome.Equals(nome, StringComparison.InvariantCultureIgnoreCase) && x.Tipo.Equals(tipo, StringComparison.InvariantCultureIgnoreCase) && x.Id != idCategoria)
                : await _efContext.Categorias.AnyAsync(x => x.IdUsuario == idUsuario && x.Nome.Equals(nome, StringComparison.InvariantCultureIgnoreCase) && x.Tipo.Equals(tipo, StringComparison.InvariantCultureIgnoreCase));
        }

        public async Task<IEnumerable<Categoria>> ObterPorUsuario(int idUsuario)
        {
            return await _efContext
                   .Categorias
                   .Include(x => x.CategoriaPai)
                   .Include(x => x.CategoriasFilha)
                        .ThenInclude(y => y.CategoriasFilha)
                   .AsNoTracking()
                   .Where(x => x.IdUsuario == idUsuario && x.IdCategoriaPai == null)
                   .ToListAsync();
        }

        public async Task Inserir(Categoria categoria)
        {
            await _efContext.AddAsync(categoria);
        }

        public void Atualizar(Categoria categoria)
        {
            _efContext.Entry(categoria).State = EntityState.Modified;
        }

        public void Deletar(Categoria categoria)
        {
            foreach (var categoriaFilha in categoria.CategoriasFilha)
            {
                Deletar(categoriaFilha);
            }

            _efContext.Categorias.Remove(categoria);
        }
    }
}