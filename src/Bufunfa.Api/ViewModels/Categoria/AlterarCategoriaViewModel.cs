using JNogueira.Bufunfa.Dominio.Resources;
using System.ComponentModel.DataAnnotations;

namespace JNogueira.Bufunfa.Api.ViewModels
{
    // View model utilizado para o alterar uma categoria
    public class AlterarCategoriaViewModel
    {
        /// <summary>
        /// Id da categoria
        /// </summary>
        [Required(ErrorMessageResourceType = typeof(Mensagem), ErrorMessageResourceName = "Id_Invalido")]
        public int IdCategoria { get; set; }

        /// <summary>
        /// Nome da categoria
        /// </summary>
        [Required(ErrorMessageResourceType = typeof(CategoriaMensagem), ErrorMessageResourceName = "Nome_Obrigatorio_Nao_Informado")]
        [MaxLength(100, ErrorMessageResourceType = typeof(CategoriaMensagem), ErrorMessageResourceName = "Nome_Tamanho_Maximo_Excedido")]
        public string Nome { get; set; }

        /// <summary>
        /// ID da categoria pai da categoria
        /// </summary>
        public int? IdCategoriaPai { get; set; }

        /// <summary>
        /// Tipo da categoria ("D" para débito / "C" para crédito)
        /// </summary>
        [Required(ErrorMessage = "O tipo da categoria é obrigatório e não foi informado.")]
        [MaxLength(1, ErrorMessage = "O tipo da categoria é inválido.")]
        public string Tipo { get; set; }
    }
}
