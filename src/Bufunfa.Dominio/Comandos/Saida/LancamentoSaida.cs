using JNogueira.Bufunfa.Dominio.Entidades;
using System;
using System.Collections.Generic;

namespace JNogueira.Bufunfa.Dominio.Comandos.Saida
{
    /// <summary>
    /// Comando de sáida para as informações de um lançamento
    /// </summary>
    public class LancamentoSaida
    {
        /// <summary>
        /// ID do lançamento
        /// </summary>
        public int Id { get; }

        /// <summary>
        /// ID da parcela que originou o lançamento
        /// </summary>
        public int? IdParcela { get; }

        /// <summary>
        /// Data do lançamento
        /// </summary>
        public DateTime Data { get; }

        /// <summary>
        /// Valor do lançamento
        /// </summary>
        public decimal Valor { get; }

        /// <summary>
        /// Observação do lançamento
        /// </summary>
        public string Observacao { get; }

        /// <summary>
        /// Conta associada ao lançamento
        /// </summary>
        public ContaSaida Conta { get; }

        /// <summary>
        /// Pessoa associada ao lançamento
        /// </summary>
        public PessoaSaida Pessoa { get; }

        /// <summary>
        /// Categoria do lançamento
        /// </summary>
        public CategoriaSaida Categoria { get; }


        public LancamentoSaida(Lancamento lancamento)
        {
            if (lancamento == null)
                return;

            this.Id         = lancamento.Id;
            this.IdParcela  = lancamento.IdParcela;
            this.Data       = lancamento.Data;
            this.Valor      = lancamento.Valor;
            this.Observacao = lancamento.Observacao;
            this.Conta      = new ContaSaida(lancamento.Conta);
            this.Pessoa     = lancamento.IdPessoa.HasValue ? new PessoaSaida(lancamento.Pessoa) : null;
            this.Categoria  = new CategoriaSaida(lancamento.Categoria);
        }

        public override string ToString()
        {
            var descricao = new List<string>();

            descricao.Add(this.Conta.Nome);

            descricao.Add(this.Categoria.Caminho);

            descricao.Add(this.Data.ToString("dd/MM/yyyy"));

            descricao.Add(this.Valor.ToString("C2"));

            return string.Join(" » ", descricao);
        }
    }
}
