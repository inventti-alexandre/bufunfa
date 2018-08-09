using JNogueira.Bufunfa.Dominio.Comandos.Entrada;

namespace JNogueira.Bufunfa.Dominio.Entidades
{
    /// <summary>
    /// Classe que representa um anexo
    /// </summary>
    public class Anexo
    {
        /// <summary>
        /// ID do anexo
        /// </summary>
        public int Id { get; private set; }

        /// <summary>
        /// Id do lançamento
        /// </summary>
        public int IdLancamento { get; private set; }

        /// <summary>
        /// Id do arquivo no Google Drive
        /// </summary>
        public string IdGoogleDrive { get; private set; }

        /// <summary>
        /// Descrição do anexo
        /// </summary>
        public string Descricao { get; private set; }

        /// <summary>
        /// Nome do arquivo do anexo
        /// </summary>
        public string NomeArquivo { get; private set; }

        /// <summary>
        /// Lançamento do anexo
        /// </summary>
        public Lancamento Lancamento { get; private set; }

        private Anexo()
        {
        }

        public Anexo(CadastrarAnexoEntrada cadastrarEntrada, string idGoogleDrive)
        {
            if (cadastrarEntrada.Valido())
                return;

            this.IdLancamento  = cadastrarEntrada.IdLancamento;
            this.IdGoogleDrive = idGoogleDrive;
            this.Descricao     = cadastrarEntrada.Descricao;
            this.NomeArquivo   = cadastrarEntrada.NomeArquivo;
        }

        public override string ToString()
        {
            return $"{this.Descricao} - {this.NomeArquivo}";
        }
    }
}