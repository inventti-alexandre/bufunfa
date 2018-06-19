using JNogueira.Bufunfa.Dominio.Resources;
using System;
using System.ComponentModel.DataAnnotations;

namespace JNogueira.Bufunfa.Api.ViewModels
{
    // View model utilizado para o cadastro de um período
    public class CadastrarPeriodoViewModel
    {
        /// <summary>
        /// Nome do período
        /// </summary>
        [Required(ErrorMessageResourceType = typeof(PeriodoMensagem), ErrorMessageResourceName = "Nome_Obrigatorio_Nao_Informado")]
        [MaxLength(50, ErrorMessageResourceType = typeof(PeriodoMensagem), ErrorMessageResourceName = "Nome_Obrigatorio_Nao_Informado")]
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
