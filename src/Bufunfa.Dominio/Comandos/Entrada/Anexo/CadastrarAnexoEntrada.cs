using JNogueira.Bufunfa.Dominio.Interfaces.Comandos;
using JNogueira.Bufunfa.Dominio.Resources;
using JNogueira.Infraestrutura.NotifiqueMe;
using System;

namespace JNogueira.Bufunfa.Dominio.Comandos.Entrada
{
    /// <summary>
    /// Comando utilizado para o cadastro de um novo anexo
    /// </summary>
    public class CadastrarAnexoEntrada : Notificavel, IEntrada
    {
        /// <summary>
        /// Id do usuário proprietário
        /// </summary>
        public int IdUsuario { get; }

        /// <summary>
        /// Id do lançamento
        /// </summary>
        public int IdLancamento { get; }

        /// <summary>
        /// Descrição do anexo
        /// </summary>
        public string Descricao { get; }

        /// <summary>
        /// Nome do arquivo do anexo
        /// </summary>
        public string NomeArquivo { get; }

        /// <summary>
        /// Conteúdo do arquivo do anexo
        /// </summary>
        public byte[] ConteudoArquivo { get; }

        /// <summary>
        /// Mime type do arquivo (necessário para realizar o upload para o Google Drive)
        /// </summary>
        public string MimeTypeArquivo { get; }

        public CadastrarAnexoEntrada(int idUsuario, int idLancamento, string descricao, string nomeArquivo, byte[] conteudoArquivo, string mimeTypeArquivo)
        {
            this.IdUsuario       = idUsuario;
            this.IdLancamento    = idLancamento;
            this.Descricao       = descricao;
            this.NomeArquivo     = nomeArquivo;
            this.ConteudoArquivo = conteudoArquivo;
            this.MimeTypeArquivo = mimeTypeArquivo;

            this.Validar();
        }

        private void Validar()
        {
            this
                .NotificarSeMenorOuIgualA(this.IdUsuario, 0, Mensagem.Id_Usuario_Invalido)
                .NotificarSeMenorOuIgualA(this.IdLancamento, 0, LancamentoMensagem.Id_Lancamento_Invalido)
                .NotificarSeNuloOuVazio(this.Descricao, AnexoMensagem.Descricao_Obrigatorio_Nao_Informado)
                .NotificarSeNuloOuVazio(this.NomeArquivo, AnexoMensagem.Nome_Arquivo_Obrigatorio_Nao_Informado)
                .NotificarSeIguais(this.ConteudoArquivo.Length, 0, AnexoMensagem.Arquivo_Conteudo_Nao_Informado);

            if (!string.IsNullOrEmpty(this.Descricao))
                this.NotificarSePossuirTamanhoSuperiorA(this.Descricao, 200, AnexoMensagem.Descricao_Tamanho_Maximo_Excedido);

            if (!string.IsNullOrEmpty(this.NomeArquivo))
                this.NotificarSePossuirTamanhoSuperiorA(this.NomeArquivo, 50, AnexoMensagem.Nome_Arquivo_Tamanho_Maximo_Excedido);

            if (this.ConteudoArquivo != null)
                this.NotificarSeVerdadeiro((decimal)(this.ConteudoArquivo.Length / 1024) > (5 * 1024), string.Format(AnexoMensagem.Arquivo_Tamanho_Nao_Permitido, Math.Round((decimal)(this.ConteudoArquivo.Length / 1024) / 1024, 1)));
        }
    }
}
