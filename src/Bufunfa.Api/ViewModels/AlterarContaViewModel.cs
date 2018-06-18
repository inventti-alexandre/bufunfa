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
        [Required(ErrorMessage = "O nome da conta é obrigatório e não foi informado.")]
        public string Nome { get; set; }

        /// <summary>
        /// Valor inicial do saldo da conta
        /// </summary>
        public decimal? ValorSaldoInicial { get; set; }

        /// <summary>
        /// Nome da instituição financeira a qual a conta pertence
        /// </summary>
        public string NomeInstituicao { get; set; }

        /// <summary>
        /// Número da agência da conta
        /// </summary>
        public string NumeroAgencia { get; set; }

        /// <summary>
        /// Número da conta
        /// </summary>
        public string Numero { get; set; }
    }
}
