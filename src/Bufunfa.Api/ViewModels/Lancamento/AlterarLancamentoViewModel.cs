using JNogueira.Bufunfa.Dominio.Resources;
using System;
using System.ComponentModel.DataAnnotations;

namespace JNogueira.Bufunfa.Api.ViewModels
{
    // View model utilizado para a alteração de um lançamento
    public class AlterarLancamentoViewModel
    {
        /// <summary>
        /// Id do lançamento
        /// </summary>
        [Required(ErrorMessageResourceType = typeof(Mensagem), ErrorMessageResourceName = "Id_Invalido")]
        public int? IdLancamento { get; set; }

        /// <summary>
        /// Id da conta
        /// </summary>
        [Required(ErrorMessageResourceType = typeof(LancamentoMensagem), ErrorMessageResourceName = "Id_Conta_Invalido")]
        public int? IdConta { get; set; }

        /// <summary>
        /// Id da categoria
        /// </summary>
        [Required(ErrorMessageResourceType = typeof(LancamentoMensagem), ErrorMessageResourceName = "Id_Categoria_Invalido")]
        public int? IdCategoria { get; set; }

        /// <summary>
        /// Id da pessoa
        /// </summary>
        public int? IdPessoa { get; set; }

        /// <summary>
        /// Data do lançamento
        /// </summary>
        [Required(ErrorMessage = "A data do lançamento é obrigatória e não foi informada.")]
        public DateTime? Data { get; set; }

        /// <summary>
        /// Data do lançamento
        /// </summary>
        [Required(ErrorMessage = "O valor do lançamento é obrigatório e não foi informado.")]
        public decimal? Valor { get; set; }

        /// <summary>
        /// Observação sobre o lançamento
        /// </summary>
        [MaxLength(500, ErrorMessageResourceType = typeof(LancamentoMensagem), ErrorMessageResourceName = "Observacao_Tamanho_Maximo_Excedido")]
        public string Observacao { get; set; }
    }
}
