using JNogueira.Bufunfa.Dominio.Resources;
using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;

namespace JNogueira.Bufunfa.Api.ViewModels
{
    // View model utilizado para o cadastro de um anexo
    public class CadastrarAnexoViewModel
    {
        /// <summary>
        /// Id do lançamento
        /// </summary>
        [Required(ErrorMessageResourceType = typeof(AnexoMensagem), ErrorMessageResourceName = "Id_Lancamento_Invalido")]
        public int? IdLancamento { get; set; }

        /// <summary>
        /// Descrição do anexo
        /// </summary>
        [Required(ErrorMessageResourceType = typeof(AnexoMensagem), ErrorMessageResourceName = "Descricao_Obrigatorio_Nao_Informado")]
        [MaxLength(200, ErrorMessageResourceType = typeof(AnexoMensagem), ErrorMessageResourceName = "Descricao_Tamanho_Maximo_Excedido")]
        public string Descricao { get; set; }

        /// <summary>
        /// Nome do arquivo do anexo
        /// </summary>
        [Required(ErrorMessageResourceType = typeof(AnexoMensagem), ErrorMessageResourceName = "Nome_Arquivo_Obrigatorio_Nao_Informado")]
        [MaxLength(50, ErrorMessageResourceType = typeof(AnexoMensagem), ErrorMessageResourceName = "Nome_Arquivo_Tamanho_Maximo_Excedido")]
        public string NomeArquivo { get; set; }

        /// <summary>
        /// Conteúdo do arquivo do anexo
        /// </summary>
        [Required(ErrorMessageResourceType = typeof(AnexoMensagem), ErrorMessageResourceName = "Arquivo_Conteudo_Nao_Informado")]
        public IFormFile Arquivo { get; set; }
    }
}
