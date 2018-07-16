using JNogueira.Bufunfa.Dominio.Resources;
using System.ComponentModel.DataAnnotations;

namespace JNogueira.Bufunfa.Api.ViewModels
{
    // View model utilizado para o descarte de uma parcela
    public class DescartarParcelaViewModel
    {
        /// <summary>
        /// Id da parcela
        /// </summary>
        [Required(ErrorMessageResourceType = typeof(ParcelaMensagem), ErrorMessageResourceName = "Id_Pessoa_Lancar_Parcela_Invalido")]
        public int? IdParcela { get; set; }

        /// <summary>
        /// Motivo do descarte da parcela
        /// </summary>
        [MaxLength(500, ErrorMessageResourceType = typeof(ParcelaMensagem), ErrorMessageResourceName = "Motivo_Descarte_Tamanho_Maximo_Excedido")]
        public string MotivoDescarte { get; set; }
    }
}
