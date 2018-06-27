using JNogueira.Bufunfa.Dominio.Resources;
using System;
using System.ComponentModel.DataAnnotations;

namespace JNogueira.Bufunfa.Api.ViewModels
{
    // View model utilizado para o alterar um período
    public class AlterarPeriodoViewModel
    {
        /// <summary>
        /// Id do usuário proprietário
        /// </summary>
        [Required(ErrorMessageResourceType = typeof(PeriodoMensagem), ErrorMessageResourceName = "Id_Periodo_Nao_Informado")]
        public int IdPeriodo { get; set; }

        /// <summary>
        /// Nome do período
        /// </summary>
        [Required(ErrorMessageResourceType = typeof(PeriodoMensagem), ErrorMessageResourceName = "Nome_Obrigatorio_Nao_Informado")]
        [MaxLength(50, ErrorMessageResourceType = typeof(PeriodoMensagem), ErrorMessageResourceName = "Nome_Tamanho_Maximo_Excedido")]
        public string Nome { get; set; }

        /// <summary>
        /// Data início do período
        /// </summary>
        [Required(ErrorMessageResourceType = typeof(PeriodoMensagem), ErrorMessageResourceName = "Data_Inicio_Obrigatoria_Nao_Informada")]
        public DateTime? DataInicio { get; set; }

        /// <summary>
        /// Data fim do período
        /// </summary>
        [Required(ErrorMessageResourceType = typeof(PeriodoMensagem), ErrorMessageResourceName = "Data_Fim_Obrigatoria_Nao_Informada")]
        public DateTime? DataFim { get; set; }
    }
}
