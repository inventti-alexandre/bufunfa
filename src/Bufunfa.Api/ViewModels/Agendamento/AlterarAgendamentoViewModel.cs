using JNogueira.Bufunfa.Dominio;
using JNogueira.Bufunfa.Dominio.Resources;
using System;
using System.ComponentModel.DataAnnotations;

namespace JNogueira.Bufunfa.Api.ViewModels
{
    // View model utilizado para alterar um agendamento
    public class AlterarAgendamentoViewModel
    {
        /// <summary>
        /// Id do agendamento
        /// </summary>
        [Required(ErrorMessageResourceType = typeof(Mensagem), ErrorMessageResourceName = "Id_Invalido")]
        public int? IdAgendamento { get; set; }

        /// <summary>
        /// Id da conta
        /// </summary>
        public int? IdConta { get; set; }

        /// <summary>
        /// Id do cartão de crédito
        /// </summary>
        public int? IdCartaoCredito { get; set; }

        /// <summary>
        /// Id da categoria
        /// </summary>
        [Required(ErrorMessage = "O ID da categoria do agendamento é obrigatório e não foi informado.")]
        public int? IdCategoria { get; set; }

        /// <summary>
        /// Id da pessoa
        /// </summary>
        public int? IdPessoa { get; set; }

        /// <summary>
        /// Tipo do método de pagamento das parcelas
        /// </summary>
        [Required(ErrorMessage = "O tipo do método de pagemento é obrigatório e não foi informado.")]
        public MetodoPagamento? TipoMetodoPagamento { get; set; }

        /// <summary>
        /// Observação sobre o agendamento
        /// </summary>
        [MaxLength(500, ErrorMessageResourceType = typeof(AgendamentoMensagem), ErrorMessageResourceName = "Observacao_Tamanho_Maximo_Excedido")]
        public string Observacao { get; set; }

        /// <summary>
        /// Valor de cada parcela do agendamento
        /// </summary>
        [Required(ErrorMessage = "O valor da parcela agendamento é obrigatório e não foi informado.")]
        public decimal? ValorParcela { get; set; }

        /// <summary>
        /// Data da primeira parcela do agendamento
        /// </summary>
        [Required(ErrorMessage = "A data da primeira parcela do agendamento é obrigatória e não foi informada.")]
        public DateTime? DataPrimeiraParcela { get; set; }

        /// <summary>
        /// Quantidade de parcelas
        /// </summary>
        [Required(ErrorMessage = "A quantidade de parcelas do agendamento é obrigatória e não foi informada.")]
        public int? QuantidadeParcelas { get; set; }

        /// <summary>
        /// Periodicidade das parcelas
        /// </summary>
        [Required(ErrorMessage = "A periodicidade das parcelas do agendamento é obrigatória e não foi informada.")]
        public Periodicidade? PeriodicidadeParcelas { get; set; }
    }
}
