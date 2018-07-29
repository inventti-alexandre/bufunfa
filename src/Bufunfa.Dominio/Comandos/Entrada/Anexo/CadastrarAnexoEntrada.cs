using JNogueira.Bufunfa.Dominio.Interfaces.Comandos;
using JNogueira.Bufunfa.Dominio.Resources;
using JNogueira.Infraestrutura.NotifiqueMe;

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

        public CadastrarAnexoEntrada(int idUsuario, int idLancamento, string descricao, string nomeArquivo)
        {
            this.IdUsuario    = idUsuario;
            this.IdLancamento = idLancamento;
            this.Descricao    = descricao;
            this.NomeArquivo  = nomeArquivo;
        }

        public bool Valido()
        {
            this
                .NotificarSeMenorOuIgualA(this.IdUsuario, 0, string.Format(Mensagem.Id_Usuario_Invalido, this.IdUsuario))
                .NotificarSeMenorOuIgualA(this.IdLancamento, 0, string.Format(AnexoMensagem.Id_Lancamento_Invalido, this.IdLancamento))
                .NotificarSeNuloOuVazio(this.Descricao, AnexoMensagem.Descricao_Obrigatorio_Nao_Informado)
                .NotificarSeNuloOuVazio(this.NomeArquivo, AnexoMensagem.Nome_Arquivo_Obrigatorio_Nao_Informado);

            if (!string.IsNullOrEmpty(this.Descricao))
                this.NotificarSePossuirTamanhoSuperiorA(this.Descricao, 200, AnexoMensagem.Descricao_Tamanho_Maximo_Excedido);

            if (!string.IsNullOrEmpty(this.NomeArquivo))
                this.NotificarSePossuirTamanhoSuperiorA(this.NomeArquivo, 50, AnexoMensagem.Nome_Arquivo_Tamanho_Maximo_Excedido);

            return !this.Invalido;
        }
    }
}
