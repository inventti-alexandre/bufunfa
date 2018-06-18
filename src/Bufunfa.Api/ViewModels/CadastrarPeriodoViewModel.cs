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
        [Required(ErrorMessage = "O nome do período é obrigatório e não foi informado.")]
        public string Nome { get; set; }

        /// <summary>
        /// Data início do período
        /// </summary>
        [Required(ErrorMessage = "A data início do período é obrigatória e não foi informada.")]
        public DateTime? DataInicio { get; set; }

        /// <summary>
        /// Data fim do período
        /// </summary>
        [Required(ErrorMessage = "A data fim do período é obrigatória e não foi informada.")]
        public DateTime? DataFim { get; set; }
    }
}
