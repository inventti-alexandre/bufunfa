using JNogueira.Bufunfa.Dominio.Entidades;
using System;
using System.Collections.Generic;
using System.Linq;

namespace JNogueira.Bufunfa.Dominio.Comandos.Saida
{
    /// <summary>
    /// Comando de sáida para as informações de um agendamento
    /// </summary>
    public class AgendamentoSaida
    {
        /// <summary>
        /// ID do agendamento
        /// </summary>
        public int Id { get; }

        /// <summary>
        /// Tipo do método de pagamento
        /// </summary>
        public MetodoPagamento TipoMetodoPagamento { get; }

        /// <summary>
        /// Observação sobre o agendamento
        /// </summary>
        public string Observacao { get; }

        /// <summary>
        /// Conta associada ao agendamento
        /// </summary>
        public ContaSaida Conta { get; }

        /// <summary>
        /// Cartão de crédito associado ao agendamento
        /// </summary>
        public CartaoCreditoSaida CartaoCredito { get; }

        /// <summary>
        /// Pessoa associada ao agendamento
        /// </summary>
        public PessoaSaida Pessoa { get; }

        /// <summary>
        /// Categoria do agendamento
        /// </summary>
        public CategoriaSaida Categoria { get; }

        /// <summary>
        /// Parcelas do agendamento
        /// </summary>
        public IEnumerable<ParcelaSaida> Parcelas { get; }

        /// <summary>
        /// Data de vencimento da próxima parcela aberta.
        /// </summary>
        public DateTime? DataProximaParcelaAberta { get; }

        /// <summary>
        /// Data de vencimento da última parcela aberta.
        /// </summary>
        public DateTime? DataUltimaParcelaAberta { get; }

        /// <summary>
        /// Quantidade total de parcelas.
        /// </summary>
        public int QuantidadeParcelas { get; }

        /// <summary>
        /// Quantidade total de parcelas abertas.
        /// </summary>
        public int QuantidadeParcelasAbertas { get; }

        /// <summary>
        /// Quantidade total de parcelas fechadas.
        /// </summary>
        public int QuantidadeParcelasFechadas { get; }

        /// <summary>
        /// Indica se o agendamento foi concluído, isto é, o número total de parcelas é igual ao número de parcelas descartadas e lançadas.
        /// </summary>
        public bool Concluido { get; }

        public AgendamentoSaida(Agendamento agendamento)
        {
            if (agendamento == null)
                return;

            this.Id                         = agendamento.Id;
            this.TipoMetodoPagamento        = agendamento.TipoMetodoPagamento;
            this.Observacao                 = agendamento.Observacao;
            this.Conta                      = agendamento.IdConta.HasValue ? new ContaSaida(agendamento.Conta) : null;
            this.CartaoCredito              = agendamento.IdCartaoCredito.HasValue ? new CartaoCreditoSaida(agendamento.CartaoCredito) : null;
            this.Pessoa                     = agendamento.IdPessoa.HasValue ? new PessoaSaida(agendamento.Pessoa) : null;
            this.Categoria                  = new CategoriaSaida(agendamento.Categoria);
            this.Parcelas                   = agendamento.Parcelas.Select(x => new ParcelaSaida(x));
            this.DataProximaParcelaAberta   = agendamento.DataProximaParcelaAberta;
            this.DataUltimaParcelaAberta    = agendamento.DataUltimaParcelaAberta;
            this.QuantidadeParcelas         = agendamento.QuantidadeParcelas;
            this.QuantidadeParcelasAbertas  = agendamento.QuantidadeParcelasAbertas;
            this.QuantidadeParcelasFechadas = agendamento.QuantidadeParcelasFechadas;
            this.Concluido                  = agendamento.Concluido;
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
