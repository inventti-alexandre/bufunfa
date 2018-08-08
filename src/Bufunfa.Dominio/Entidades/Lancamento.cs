using JNogueira.Bufunfa.Dominio.Comandos.Entrada;
using System;
using System.Collections.Generic;

namespace JNogueira.Bufunfa.Dominio.Entidades
{
    /// <summary>
    /// Classe que representa um lançamento
    /// </summary>
    public class Lancamento
    {
        /// <summary>
        /// ID do lançamento
        /// </summary>
        public int Id { get; private set; }

        /// <summary>
        /// Id do usuário proprietário
        /// </summary>
        public int IdUsuario { get; private set; }

        /// <summary>
        /// Id da conta
        /// </summary>
        public int IdConta { get; private set; }

        /// <summary>
        /// Id da categoria
        /// </summary>
        public int IdCategoria { get; private set; }

        /// <summary>
        /// Id da pessoa
        /// </summary>
        public int? IdPessoa { get; private set; }

        /// <summary>
        /// Id da parcela que ocasionou o lançamento
        /// </summary>
        public int? IdParcela { get; private set; }

        /// <summary>
        /// Data do lançamento
        /// </summary>
        public DateTime Data { get; private set; }

        /// <summary>
        /// Valor do lançamento
        /// </summary>
        public decimal Valor { get; private set; }

        /// <summary>
        /// Observação do lançamento
        /// </summary>
        public string Observacao { get; private set; }

        /// <summary>
        /// Conta associada ao lançamento
        /// </summary>
        public Conta Conta { get; private set; }

        /// <summary>
        /// Categoria associada ao lançamento
        /// </summary>
        public Categoria Categoria { get; private set; }

        /// <summary>
        /// Pessoa associada ao lançamento
        /// </summary>
        public Pessoa Pessoa { get; private set; }

        /// <summary>
        /// Anexos do lançamento
        /// </summary>
        public IEnumerable<Anexo> Anexos { get; private set; }

        private Lancamento()
        {
            this.Anexos = new List<Anexo>();
        }

        public Lancamento(CadastrarLancamentoEntrada cadastrarEntrada)
            : this()
        {
            if (cadastrarEntrada.Invalido)
                return;

            this.IdUsuario   = cadastrarEntrada.IdUsuario;
            this.IdConta     = cadastrarEntrada.IdConta;
            this.IdCategoria = cadastrarEntrada.IdCategoria;
            this.IdParcela   = cadastrarEntrada.IdParcela;
            this.Data        = cadastrarEntrada.Data;
            this.Valor       = cadastrarEntrada.Valor;
            this.IdPessoa    = cadastrarEntrada.IdPessoa;
            this.Observacao  = cadastrarEntrada.Observacao;
        }

        public void Alterar(AlterarLancamentoEntrada alterarEntrada)
        {
            if (!alterarEntrada.Valido() || alterarEntrada.IdLancamento != this.Id)
                return;

            this.IdConta = alterarEntrada.IdConta;
            this.IdCategoria = alterarEntrada.IdCategoria;
            this.Data = alterarEntrada.Data;
            this.Valor = alterarEntrada.Valor;
            this.IdPessoa = alterarEntrada.IdPessoa;
            this.Observacao = alterarEntrada.Observacao;
        }

        public override string ToString()
        {
            var descricao = new List<string>();

            if (this.Conta != null)
                descricao.Add(this.Conta.Nome);

            if (!string.IsNullOrEmpty(this.Observacao))
                descricao.Add(this.Observacao);

            descricao.Add(this.Data.ToString("dd/MM/yyyy"));
            descricao.Add(this.Valor.ToString("C2"));

            return string.Join(" » ", descricao);
        }
    }
}