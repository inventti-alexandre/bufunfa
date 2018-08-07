using JNogueira.Bufunfa.Dominio.Entidades;

namespace JNogueira.Bufunfa.Dominio.Comandos.Saida
{
    /// <summary>
    /// Comando de sáida para as informações de um anexo
    /// </summary>
    public class AnexoSaida
    {
        /// <summary>
        /// ID do anexo
        /// </summary>
        public int Id { get; }

        /// <summary>
        /// Id do lançamento
        /// </summary>
        public int IdLancamento { get; }

        /// <summary>
        /// Id do arquivo no Google Drive
        /// </summary>
        public string IdGoogleDrive { get; }

        /// <summary>
        /// Descrição do anexo
        /// </summary>
        public string Descricao { get; }

        /// <summary>
        /// Nome do arquivo do anexo
        /// </summary>
        public string NomeArquivo { get; }

        public AnexoSaida(Anexo anexo)
        {
            if (anexo == null)
                return;

            this.Id            = anexo.Id;
            this.IdLancamento  = anexo.IdLancamento;
            this.IdGoogleDrive = anexo.IdGoogleDrive;
            this.Descricao     = anexo.Descricao;
            this.NomeArquivo   = anexo.NomeArquivo;
        }

        public override string ToString()
        {
            return $"{this.Descricao} - {this.NomeArquivo}";
        }
    }
}
