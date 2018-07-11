using JNogueira.Bufunfa.Dominio.Comandos.Entrada;
using System;
using System.Collections.Generic;
using System.Linq;

namespace JNogueira.Bufunfa.Dominio.Entidades
{
    /// <summary>
    /// Classe que representa um agendamento
    /// </summary>
    public class Agendamento
    {
        /// <summary>
        /// Id do agendamento
        /// </summary>
        public int Id { get; private set; }

        /// <summary>
        /// Id do usuário proprietário
        /// </summary>
        public int IdUsuario { get; private set; }

        /// <summary>
        /// Id da categoria
        /// </summary>
        public int IdCategoria { get; private set; }

        /// <summary>
        /// Id da conta
        /// </summary>
        public int? IdConta { get; private set; }

        /// <summary>
        /// Id do cartão de crédito
        /// </summary>
        public int? IdCartaoCredito { get; private set; }
        
        /// <summary>
        /// Id da pessoa
        /// </summary>
        public int? IdPessoa { get; private set; }

        /// <summary>
        /// Tipo do método de pagamento
        /// </summary>
        public MetodoPagamento TipoMetodoPagamento { get; private set; }

        /// <summary>
        /// Observação sobre o agendamento
        /// </summary>
        public string Observacao { get; private set; }

        /// <summary>
        /// Conta associada ao agendamento
        /// </summary>
        public Conta Conta { get; private set; }

        /// <summary>
        /// Cartão de crédito associado ao agendamento
        /// </summary>
        public CartaoCredito CartaoCredito { get; private set; }

        /// <summary>
        /// Pessoa associada ao agendamento
        /// </summary>
        public Pessoa Pessoa { get; private set; }

        /// <summary>
        /// Categoria do agendamento
        /// </summary>
        public Categoria Categoria { get; private set; }

        /// <summary>
        /// Parcelas do agendamento
        /// </summary>
        public IEnumerable<Parcela> Parcelas { get; private set; }

        /// <summary>
        /// Data de vencimento da primeira parcela
        /// </summary>
        public DateTime? DataPrimeiraParcela
        {
            get
            {
                if (this.Parcelas == null || !this.Parcelas.Any())
                    return null;

                return this.Parcelas.Min(x => x.Data);
            }
        }

        /// <summary>
        /// Data de vencimento da última parcela.
        /// </summary>
        public DateTime? DataUltimaParcela
        {
            get
            {
                if (this.Parcelas == null || !this.Parcelas.Any())
                    return null;

                return this.Parcelas.Max(x => x.Data);
            }
        }

        /// <summary>
        /// Quantidade total de parcelas.
        /// </summary>
        public int QuantidadeParcelas
        {
            get
            {
                if (this.Parcelas == null || !this.Parcelas.Any())
                    return 0;

                return this.Parcelas.Count();
            }
        }

        /// <summary>
        /// Quantidade total de parcelas já lançadas.
        /// </summary>
        public int QuantidadeParcelasLancadas
        {
            get
            {
                if (this.Parcelas == null || !this.Parcelas.Any())
                    return 0;

                return this.Parcelas.Count(x => x.SeLancada);
            }
        }

        /// <summary>
        /// Quantidade total de parcelas descartadas.
        /// </summary>
        public int QuantidadeParcelasDescartadas
        {
            get
            {
                if (this.Parcelas == null || !this.Parcelas.Any())
                    return 0;

                return this.Parcelas.Count(x => x.SeDescartada);
            }
        }

        /// <summary>
        /// Indica se o agendamento foi concluído, isto é, o número total de parcelas é igual ao número de parcelas descartadas e lançadas.
        /// </summary>
        public bool SeConcluido
        {
            get
            {
                if (this.Parcelas == null || !this.Parcelas.Any())
                    return true;

                return this.QuantidadeParcelas == this.QuantidadeParcelasLancadas + this.QuantidadeParcelasDescartadas;
            }
        }

        private Agendamento()
        {
            this.Parcelas = new List<Parcela>();
        }

        public Agendamento(CadastrarAgendamentoEntrada cadastrarEntrada) 
            : this()
        {
            if (!cadastrarEntrada.Valido())
                return;

            this.IdUsuario           = cadastrarEntrada.IdUsuario;
            this.IdCategoria         = cadastrarEntrada.IdCategoria;
            this.IdConta             = cadastrarEntrada.IdConta;
            this.IdCartaoCredito     = cadastrarEntrada.IdCartaoCredito;
            this.IdPessoa            = cadastrarEntrada.IdPessoa;
            this.TipoMetodoPagamento = cadastrarEntrada.TipoMetodoPagamento;
            this.Observacao          = cadastrarEntrada.Observacao;
        }

        public override string ToString()
        {
            var descricao = new List<string>();

            if (this.Conta != null)
                descricao.Add(this.Conta.Nome);

            if (this.CartaoCredito != null)
                descricao.Add(this.CartaoCredito.Nome);

            if (!string.IsNullOrEmpty(this.Observacao))
                descricao.Add(this.Observacao);

            return string.Join(" » ", descricao);
        } 
    }
}
