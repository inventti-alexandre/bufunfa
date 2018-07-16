using JNogueira.Bufunfa.Dominio.Resources;
using System;
using System.ComponentModel.DataAnnotations;

namespace JNogueira.Bufunfa.Api.ViewModels
{
    // View model utilizado para a alteração de uma parcela
    public class AlterarParcelaViewModel
    {
        /// <summary>
        /// Id da parcela
        /// </summary>
        [Required(ErrorMessageResourceType = typeof(ParcelaMensagem), ErrorMessageResourceName = "Id_Parcela_Invalido")]
        public int? IdParcela { get; set; }

        /// <summary>
        /// Data da parcela
        /// </summary>
        [Required(ErrorMessage = "A data da parcela é obrigatória e não foi informada.")]
        public DateTime? Data { get; set; }

        /// <summary>
        /// Valor da parcela
        /// </summary>
        [Required(ErrorMessage = "O valor da parcela é obrigatório e não foi informado.")]
        public decimal? Valor { get; set; }

        /// <summary>
        /// Observação sobre a parcela
        /// </summary>
        [MaxLength(500, ErrorMessageResourceType = typeof(ParcelaMensagem), ErrorMessageResourceName = "Observacao_Tamanho_Maximo_Excedido")]
        public string Observacao { get; set; }
    }
}
