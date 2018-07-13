using JNogueira.Bufunfa.Dominio.Resources;
using System.ComponentModel.DataAnnotations;

namespace JNogueira.Bufunfa.Api.ViewModels
{
    // View model utilizado para alterar uma pessoa
    public class AlterarPessoaViewModel
    {
        /// <summary>
        /// Id da pessoa
        /// </summary>
        [Required(ErrorMessageResourceType = typeof(Mensagem), ErrorMessageResourceName = "Id_Invalido")]
        public int IdPessoa{ get; set; }

        /// <summary>
        /// Nome do período
        /// </summary>
        [Required(ErrorMessageResourceType = typeof(PessoaMensagem), ErrorMessageResourceName = "Nome_Obrigatorio_Nao_Informado")]
        [MaxLength(200, ErrorMessageResourceType = typeof(PessoaMensagem), ErrorMessageResourceName = "Nome_Tamanho_Maximo_Excedido")]
        public string Nome { get; set; }
    }
}
