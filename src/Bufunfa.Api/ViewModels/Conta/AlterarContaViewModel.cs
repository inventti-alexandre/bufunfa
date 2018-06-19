using JNogueira.Bufunfa.Dominio.Resources;
using System.ComponentModel.DataAnnotations;

namespace JNogueira.Bufunfa.Api.ViewModels
{
    // View model utilizado para o alterar a conta
    public class AlterarContaViewModel
    {
        /// <summary>
        /// Id da conta
        /// </summary>
        [Required(ErrorMessage = "O ID da conta é obrigatório e não foi informado.")]
        public int IdConta { get; set; }

        /// <summary>
        /// Nome da conta
        /// </summary>
        [Required(ErrorMessageResourceType = typeof(ContaMensagem), ErrorMessageResourceName = "Nome_Obrigatorio_Nao_Informado")]
        [MaxLength(100, ErrorMessageResourceType = typeof(ContaMensagem), ErrorMessageResourceName = "Nome_Tamanho_Maximo_Excedido")]
        public string Nome { get; set; }

        /// <summary>
        /// Valor inicial do saldo da conta
        /// </summary>
        public decimal? ValorSaldoInicial { get; set; }

        /// <summary>
        /// Nome da instituição financeira a qual a conta pertence
        /// </summary>
        [MaxLength(500, ErrorMessageResourceType = typeof(ContaMensagem), ErrorMessageResourceName = "Nome_Instituicao_Tamanho_Maximo_Excedido")]
        public string NomeInstituicao { get; set; }

        /// <summary>
        /// Número da agência da conta
        /// </summary>
        [MaxLength(20, ErrorMessageResourceType = typeof(ContaMensagem), ErrorMessageResourceName = "Numero_Agencia_Tamanho_Maximo_Excedido")]
        public string NumeroAgencia { get; set; }

        /// <summary>
        /// Número da conta
        /// </summary>
        [MaxLength(20, ErrorMessageResourceType = typeof(ContaMensagem), ErrorMessageResourceName = "Numero_Tamanho_Maximo_Excedido")]
        public string Numero { get; set; }
    }
}
