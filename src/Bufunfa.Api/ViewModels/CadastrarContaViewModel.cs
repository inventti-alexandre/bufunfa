using System.ComponentModel.DataAnnotations;

namespace JNogueira.Bufunfa.Api.ViewModels
{
    // View model utilizado para o cadastro de uma conta
    public class CadastrarContaViewModel
    {
        /// <summary>
        /// Id do usuário proprietário da conta
        /// </summary>
        [Required]
        public int IdUsuario { get; set; }

        /// <summary>
        /// Nome da conta
        /// </summary>
        [Required]
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
