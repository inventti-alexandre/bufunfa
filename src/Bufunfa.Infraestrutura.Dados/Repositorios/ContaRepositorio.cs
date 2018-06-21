﻿using JNogueira.Bufunfa.Dominio.Entidades;
using JNogueira.Bufunfa.Dominio.Interfaces.Dados;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace JNogueira.Bufunfa.Infraestrutura.Dados.Repositorios
{
    public class ContaRepositorio : IContaRepositorio
    {
        private readonly EfDataContext _efContext;

        public ContaRepositorio(EfDataContext efContext)
        {
            _efContext = efContext;
        }

        public async Task<Conta> ObterPorId(int idConta, bool habilitarTracking = false)
        {
            var query = _efContext.Contas.AsQueryable();

            if (!habilitarTracking)
                query = query.AsNoTracking();

            return await query.FirstOrDefaultAsync(x => x.Id == idConta);
        }

        public async Task<IEnumerable<Conta>> ObterPorUsuario(int idUsuario)
        {
            return await _efContext
                   .Contas
                   .AsNoTracking()
                   .Where(x => x.IdUsuario == idUsuario)
                   .ToListAsync();
        }

        public async Task Inserir(Conta conta)
        {
            await _efContext.AddAsync(conta);
        }

        public void Atualizar(Conta conta)
        {
            _efContext.Entry(conta).State = EntityState.Modified;
        }

        public void Deletar(Conta conta)
        {
            _efContext.Contas.Remove(conta);
        }

        public async Task<bool> VerificarExistenciaPorNome(int idUsuario, string nome, int? idConta = null)
        {
            return idConta.HasValue
                ? await _efContext.Contas.AnyAsync(x => x.IdUsuario == idUsuario && x.Nome.Equals(nome, System.StringComparison.InvariantCultureIgnoreCase) && x.Id != idConta)
                : await _efContext.Contas.AnyAsync(x => x.IdUsuario == idUsuario && x.Nome.Equals(nome, System.StringComparison.InvariantCultureIgnoreCase));
        }
    }
}
